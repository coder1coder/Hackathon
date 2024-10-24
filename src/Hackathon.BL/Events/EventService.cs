using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation;
using Hackathon.BL.Validation.Events;
using Hackathon.Common.Abstraction.ApprovalApplications;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Extensions;
using Hackathon.Common.Messages;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Teams;
using Hackathon.Common.Models.Users;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Hackathon.FileStorage.BL.Services;
using Hackathon.FileStorage.BL.Validators;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;
using Hackathon.Informing.Abstractions.Services;
using Hackathon.Informing.BL;
using Hackathon.Infrastructure;
using Hackathon.IntegrationEvents.Hubs;
using Hackathon.IntegrationEvents.IntegrationEvents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserErrorMessages = Hackathon.BL.Users.UserErrorMessages;

namespace Hackathon.BL.Events;

/// <summary>
/// События
/// </summary>
public class EventService : IEventService
{
    private readonly IValidator<EventCreateParameters> _createEventModelValidator;
    private readonly IValidator<EventUpdateParameters> _updateEventModelValidator;
    private readonly IValidator<IFileImage> _eventImageValidator;
    private readonly IEventRepository _eventRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<Common.Models.GetListParameters<EventFilter>> _getFilterModelValidator;
    private readonly ITeamService _teamService;
    private readonly INotificationService _notificationService;
    private readonly IEventChangesIntegrationEventsHub _eventChangesIntegrationEventsHub;
    private readonly IEventAgreementRepository _eventAgreementRepository;
    private readonly ILogger<EventService> _logger;
    private readonly IApprovalApplicationRepository _approvalApplicationRepository;
    private readonly IMessageBusService _messageBusService;

    public EventService(
        IValidator<EventCreateParameters> createEventModelValidator, 
        IValidator<EventUpdateParameters> updateEventModelValidator, 
        IValidator<Common.Models.GetListParameters<EventFilter>> getFilterModelValidator, 
        IValidator<IFileImage> eventImageValidator,
        IEventRepository eventRepository,
        ITeamService teamService,
        IUserRepository userRepository,
        INotificationService notificationService,
        IEventChangesIntegrationEventsHub eventChangesIntegrationEventsHub,
        IFileStorageService fileStorageService,
        IFileStorageRepository fileStorageRepository,
        IEventAgreementRepository eventAgreementRepository,
        ILogger<EventService> logger, 
        IApprovalApplicationRepository approvalApplicationRepository, 
        IMessageBusService messageBusService)
    {
        _createEventModelValidator = createEventModelValidator;
        _updateEventModelValidator = updateEventModelValidator;
        _getFilterModelValidator = getFilterModelValidator;
        _eventImageValidator = eventImageValidator;
        _eventRepository = eventRepository;
        _teamService = teamService;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _eventChangesIntegrationEventsHub = eventChangesIntegrationEventsHub;
        _fileStorageService = fileStorageService;
        _fileStorageRepository = fileStorageRepository;
        _eventAgreementRepository = eventAgreementRepository;
        _logger = logger;
        _approvalApplicationRepository = approvalApplicationRepository;
        _messageBusService = messageBusService;
    }

    public async Task<Result<long>> CreateAsync(long authorizedUserId, EventCreateParameters eventCreateParameters)
    {
        eventCreateParameters.OwnerId = authorizedUserId;
        await _createEventModelValidator.ValidateAndThrowAsync(eventCreateParameters);

        await AssignImageIdFromTemporaryFile(eventCreateParameters);

        var eventId = await _eventRepository.CreateAsync(eventCreateParameters);

        await _messageBusService.TryPublish(new EventCreatedMessage(eventId, authorizedUserId));

        return Result<long>.FromValue(eventId);
    }

    public async Task<Result> UpdateAsync(long authorizedUserId, EventUpdateParameters eventUpdateParameters)
    {
        await _updateEventModelValidator.ValidateAndThrowAsync(eventUpdateParameters);

        var eventModel = await _eventRepository.GetAsync(eventUpdateParameters.Id);

        if (eventModel is null)
        {
            return Result.NotFound(EventsErrorMessages.EventNotFound);
        }

        if (eventModel.Owner?.Id != authorizedUserId)
        {
            return Result.Forbidden(EventsErrorMessages.NoRightsExecuteOperation);
        }

        if (eventModel.Status != EventStatus.Draft && eventModel.Status != EventStatus.OnModeration)
        {
            return Result.NotValid(EventsErrorMessages.IncorrectStatusForUpdating);
        }

        if (eventModel.Status == EventStatus.OnModeration)
        {
            if (eventModel.ApprovalApplicationId.HasValue)
            {
                await _approvalApplicationRepository.RemoveAsync(eventModel.ApprovalApplicationId.Value);
            }

            eventModel.Status = EventStatus.Draft;
        }

        if (eventModel.ImageId.GetValueOrDefault() != eventUpdateParameters.ImageId.GetValueOrDefault())
        {
            //Помечаем старый файл, если такой существует, как удаленный
            if (eventModel.ImageId.HasValue)
            {
                await _fileStorageRepository.UpdateFlagIsDeleted(eventModel.ImageId.Value, true);
            }

            await AssignImageIdFromTemporaryFile(eventUpdateParameters);
        }

        await _eventRepository.UpdateAsync(eventUpdateParameters);

        return Result.Success;
    }

    public async Task<Result<EventModel>> GetAsync(long? authorizedUserId, long eventId)
    {
        var @event = await _eventRepository.GetAsync(eventId);
        if (@event is null)
        {
            return Result<EventModel>.NotFound(EventsErrorMessages.EventNotFound);
        }

        var userModel = await _userRepository.GetAsync(authorizedUserId.GetValueOrDefault());

        var canReadEvent =
            @event.Status is EventStatus.OnModeration && userModel?.Role is UserRole.Administrator
            || @event.Owner?.Id == authorizedUserId.GetValueOrDefault()
            || @event.Status is EventStatus.Published or EventStatus.Started or EventStatus.Finished;

        return !canReadEvent
            ? Result<EventModel>.Forbidden(EventsErrorMessages.NoRightsExecuteOperation)
            : Result<EventModel>.FromValue(@event);
    }

    public async Task<Result<BaseCollection<EventListItem>>> GetListAsync(long authorizedUserId, Common.Models.GetListParameters<EventFilter> parameters)
    {
        await _getFilterModelValidator.ValidateAndThrowAsync(parameters);

        var user = await _userRepository.GetAsync(authorizedUserId);
        if (user is null)
        {
            return Result<BaseCollection<EventListItem>>.Forbidden(UserErrorMessages.UserDoesNotExists);
        }

        parameters.Filter ??= new EventFilter();
        parameters.Filter.ExcludeOtherUsersEventsByStatuses = new[]
        {
            EventStatus.Draft,
            EventStatus.OnModeration
        };

        var events = await _eventRepository.GetListAsync(authorizedUserId, parameters);

        return Result<BaseCollection<EventListItem>>.FromValue(new BaseCollection<EventListItem>
        {
            TotalCount = events.TotalCount,
            Items = events.Items?
                .ToArray()
        });
    }

    public async Task<Result> SetStatusAsync(
        long authorizedUserId,
        long eventId,
        EventStatus eventStatus,
        bool skipValidation = false,
        bool skipUserValidation = false,
        UserRole skipUserValidationRole = UserRole.Default)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel == null)
        {
            return Result.NotFound(EventsErrorMessages.EventNotFound);
        }

        var userRole = skipUserValidationRole;

        if (!skipUserValidation)
        {
            var authorizedUserRole = await _userRepository.GetRoleAsync(authorizedUserId);

            if (authorizedUserRole is null)
            {
                return Result.Forbidden(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);
            }

            userRole = authorizedUserRole.Value;
        }

        if (!skipValidation)
        {
            var (isValid, errorMessage) = ChangeEventStatusValidator.ValidateAsync(authorizedUserId, userRole, eventModel, eventStatus);

            if (!isValid)
            {
                return Result.NotValid(errorMessage);
            }
        }

        switch (eventStatus)
        {
            case EventStatus.OnModeration:
                await _approvalApplicationRepository.AddAsync(new NewApprovalApplicationParameters
                {
                    EventId = eventModel.Id,
                    AuthorId = authorizedUserId,
                    ApplicationStatus = ApprovalApplicationStatus.Requested,
                    RequestedAt = DateTimeOffset.UtcNow
                });
                break;

            case EventStatus.Started:
            {
                var initialStageId = eventModel.Stages.MinBy(x => x.Order)?.Id ?? default;
                await _eventRepository.SetCurrentStageId(eventId, initialStageId);
                break;
            }

            case EventStatus.Draft:
            case EventStatus.Published:
            case EventStatus.Finished:
            default:
                //nothing
                break;
        }

        await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);

        return Result.Success;
    }

    public async Task<Result> JoinAsync(long eventId, long userId)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel.Agreement?.RequiresConfirmation == true
            && eventModel.Agreement?.Users?.Any(x=>x.Id == userId) != true)
        {
            return Result.NotValid(EventsErrorMessages.AgreementShouldBeSigned);
        }

        if (eventModel.Status != EventStatus.Published)
        {
            return Result.NotValid(EventsErrorMessages.CantAttachToEvent);
        }

        var notFullTeams = GetNotFullTeams(eventModel);

        if (notFullTeams.Count == 0 && !eventModel.IsCreateTeamsAutomatically)
        {
            return Result.NotValid(EventsErrorMessages.CantAttachToEvent);
        }

        long teamId;

        if (notFullTeams.Count > 0)
        {
            teamId = notFullTeams.First().Id;
        }
        else
        {
            var createTeamResult = await _teamService.CreateAsync(new CreateTeamModel
            {
                EventId = eventId,
                Name = GenerateAutoCreatedTeamName(eventId)
            });

            if (!createTeamResult.IsSuccess)
            {
                return Result.FromErrors(createTeamResult.Errors);
            }

            teamId = createTeamResult.Data;
        }

        var addMemberResult = await _teamService.AddMemberAsync(new TeamMemberModel
        {
            TeamId = teamId,
            MemberId = userId,
            Role = TeamRole.Participant
        });

        return !addMemberResult.IsSuccess
            ? addMemberResult
            : Result.Success;
    }

    /// <summary>
    /// Покинуть событие
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="authorizedUserId">Идентификатор пользователя</param>
    public async Task<Result> LeaveAsync(long eventId, long authorizedUserId)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel is null)
        {
            return Result.NotValid(EventsErrorMessages.EventNotFound);
        }

        if (eventModel.Status != EventStatus.Published)
        {
            return Result.NotValid(EventsErrorMessages.CantLeaveEventWhenItsAlreadyStarted);
        }

        var userExists = await _userRepository.ExistsAsync(authorizedUserId);

        if (!userExists)
        {
            return Result.NotValid(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);
        }

        var userTeam = GetTeamContainsMember(eventModel, authorizedUserId);
        if (userTeam == null)
        {
            return Result.NotValid(EventsErrorMessages.UserIsNotInEvent);
        }

        if (userTeam.OwnerId.HasValue)
        {
            return Result.NotValid(EventsErrorMessages.CantLeaveEventIfYouEnteredAsTeam);
        }

        await _teamService.RemoveMemberAsync(new TeamMemberModel
        {
            TeamId = userTeam.Id,
            MemberId = authorizedUserId
        });

        return Result.Success;
    }

    public async Task<Result> DeleteAsync(long eventId, long userId)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel is null)
        {
            return Result.NotValid(EventsErrorMessages.EventNotFound);
        }

        if (eventModel.Owner?.Id != userId)
        {
            return Result.NotValid(EventsErrorMessages.CantDeleteEventUserWhoNotOwner);
        }

        if (eventModel.Status is not (EventStatus.Draft or EventStatus.OnModeration))
        {
            return Result.NotValid(EventsErrorMessages.CantDeleteEventWithStatusOtherThаnDraftOnModeration);
        }

        await _approvalApplicationRepository.RemoveAsync(eventModel.ApprovalApplicationId.GetValueOrDefault());
        await _eventRepository.DeleteAsync(eventId);
        return Result.Success;
    }

    public async Task<Result<BaseCollection<EventListItem>>> GetUpcomingEventsAsync(TimeSpan timeBeforeStart)
    {
        var now = DateTime.UtcNow.AddTicks(-timeBeforeStart.Ticks).ToUtcWithoutSeconds();

        var result = await _eventRepository.GetListAsync(default, new Common.Models.GetListParameters<EventFilter>
        {
            Filter = new EventFilter
            {
                StartFrom = now,
                StartTo = now.AddMinutes(1),
                Statuses = new [] {EventStatus.Published }
            }
        });

        return Result<BaseCollection<EventListItem>>.FromValue(result);
    }

    public async Task<Result> GoNextStage(long userId, long eventId)
    {
        var userExists = await _userRepository.ExistsAsync(userId);
        if (!userExists)
        {
            return Result.NotValid(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);
        }

        var eventModel = await _eventRepository.GetAsync(eventId);
        if (eventModel is null)
        {
            return Result.NotValid(EventsErrorMessages.EventNotFound);
        }

        if (eventModel.Owner?.Id != userId)
        {
            return Result.Forbidden("Только владелец события может переключить этап");
        }

        var notAvailableStatusesForGoingNextStage = new[] {EventStatus.Draft, EventStatus.Published, EventStatus.Finished};

        if (notAvailableStatusesForGoingNextStage.Contains(eventModel.Status))
        {
            return Result.NotValid("Текущий статус события не позволяет перейти к следующему этапу");
        }

        var orderedStages = eventModel.Stages.OrderBy(x => x.Order).ToList();
        var currentStageIndex = orderedStages.FindIndex(x => x.Id == eventModel.CurrentStageId);

        if (currentStageIndex < 0)
        {
            return Result.Internal("Невозможно определить текущее событие");
        }

        if (orderedStages.Count - 1 == currentStageIndex)
        {
            return Result.NotValid("Текущий этап события является последним");
        }

        var nextStageId = orderedStages[++currentStageIndex].Id;
        await _eventRepository.SetCurrentStageId(eventId, nextStageId);

        await _eventChangesIntegrationEventsHub.PublishAll(new EventStageChangedIntegrationEvent(eventId, nextStageId));
        return Result.Success;
    }

    /// <summary>
    /// Загрузить изображение события
    /// </summary>
    /// <param name="file">Файл http запроса</param>
    public async Task<Result<Guid>> UploadEventImageAsync(IFormFile file)
    {
        if (file is null)
        {
            return Result<Guid>.NotValid(EventsErrorMessages.EventFileImageIsNotBeEmpty);
        }

        await using var stream = file.OpenReadStream();

        var image = await ImageLoader.LoadFromStreamAsync(stream, file.FileName, fileImage => 
            new EventFileImage(fileImage.Width, fileImage.Height, fileImage.Length, fileImage.Extension));

        await _eventImageValidator.ValidateAndThrowAsync(image, options =>
            options.IncludeRuleSets(FileImageValidatorRuleSets.EventImage));

        var uploadResult = await _fileStorageService.UploadAsync(stream, Bucket.Events, file.FileName, null, true);

        return Result<Guid>.FromValue(uploadResult.Id);
    }

    public async Task<Result<EventAgreementModel>> GetAgreementAsync(long eventId)
    {
        var agreement = await _eventAgreementRepository.GetByEventId(eventId);
        return agreement is null
            ? Result<EventAgreementModel>.NotValid(EventsErrorMessages.AgreementDoesNotExists)
            : Result<EventAgreementModel>.FromValue(agreement);
    }

    public async Task<Result> AcceptAgreementAsync(long authorizedUserId, long eventId)
    {
        var user = await _userRepository.GetAsync(authorizedUserId);
        if (user is null)
        {
            return Result.NotValid(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);
        }

        var agreement = await _eventAgreementRepository.GetByEventId(eventId);
        if (agreement is null)
        {
            return Result.NotValid(EventsErrorMessages.AgreementDoesNotExists);
        }

        if (agreement.Event is null)
        {
            return Result.NotValid(EventsErrorMessages.EventNotFound);
        }

        if (agreement.Event.Status is not EventStatus.Published)
        {
            return Result.NotValid("Мероприятие должно быть опубликовано");
        }

        if (agreement.Users.Any(x=>x.Id == authorizedUserId))
        {
            return Result.Success;
        }

        if (!agreement.RequiresConfirmation)
        {
            return Result.Success;
        }

        agreement.Users.Add(user);

        await _eventAgreementRepository.UpsertUserRelationAsync(agreement.EventId, authorizedUserId);
        return Result.Success;
    }

    /// <summary>
    /// Меняет статус события и отправляет сообщение в шину с уведомлением участников события
    /// </summary>
    /// <param name="eventModel"></param>
    /// <param name="eventStatus"></param>
    private async Task ChangeEventStatusAndPublishMessage(EventModel eventModel, EventStatus eventStatus)
    {
        await _eventRepository.SetStatusAsync(eventModel.Id, eventStatus);

        await _eventChangesIntegrationEventsHub.PublishAll(new EventStatusChangedIntegrationEvent(eventModel.Id, eventStatus));

        if (eventStatus == EventStatus.OnModeration)
        {
            await NotifyAdministrators(eventModel);
        }

        await NotifyEventMembers(eventModel, eventStatus);
    }

    private async Task NotifyAdministrators(EventModel eventModel)
    {
        var administratorIds = await _userRepository.GetAdministratorIdsAsync();
        if (administratorIds is not {Length: > 0})
        {
            return;
        }

        var notifications = administratorIds.Select(administratorId =>
            NotificationCreator.System(new SystemNotificationData($"Заявка на согласование мероприятия <{eventModel.Name}>"),
                eventModel.OwnerId, administratorId));

        await _notificationService.PushManyAsync(notifications);
    }

    private async Task NotifyEventMembers(EventModel eventModel, EventStatus eventStatus)
    {
        var eventStatusChangingMessage = ResolveEventStatusChangingMessage(eventModel, eventStatus);
        if (eventStatusChangingMessage is null)
        {
            return;
        }

        var usersIds = eventModel.Teams
            .SelectMany(x => x.Members?.Select(z => z.Id))
            .ToArray();

        if (!usersIds.Any())
        {
            return;
        }

        var notificationModels = usersIds.Select(userId =>
            NotificationCreator.System(new SystemNotificationData(eventStatusChangingMessage), userId));

        await _notificationService.PushManyAsync(notificationModels);
    }

    private static string ResolveEventStatusChangingMessage(BaseEventParameters eventModel, EventStatus eventStatus)
    {
        var eventStatusChangingMessageByOwner = eventModel.ChangeEventStatusMessages
            .FirstOrDefault(x => x.Status == eventStatus)
            ?.Message;
        
        return eventStatusChangingMessageByOwner ?? eventStatus switch
        {
            EventStatus.Started => $"Событие '{eventModel.Name}' началось",
            EventStatus.Finished => $"Событие '{eventModel.Name}' завершено",
            _ => null
        };
    }

    private static TeamModel GetTeamContainsMember(EventModel eventModel, long userId)
        => eventModel.Teams
            .FirstOrDefault(x =>
                x.Members?.Any(s=>s.Id == userId) == true);

    /// <summary>
    /// Сформировать наименование для команды создаваемой автоматически
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    private static string GenerateAutoCreatedTeamName(long eventId)
        => $"Team-{eventId}-{Guid.NewGuid().ToString()[..4]}";

    private async Task AssignImageIdFromTemporaryFile(BaseEventParameters parameters)
    {
        if (!parameters.ImageId.HasValue)
        {
            return;
        }

        StorageFile fileModel = null;

        try
        {
            fileModel = await _fileStorageRepository.GetAsync(parameters.ImageId.Value);

            if (fileModel is not null)
            {
                //Снимаем ранее установленный флаг, чтобы не удалить фотографию события
                await _fileStorageRepository.UpdateFlagIsDeleted(fileModel.Id, false);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Source}: ошибка во время удаления флага - временный файл у StorageFile: {StorageFileId}",
                        nameof(EventService), fileModel?.Id);
        }
        finally
        {
            parameters.ImageId = fileModel?.Id;
        }
    }
    
    /// <summary>
    /// Получить команды связанные с событием, которые заполнены не полностью
    /// </summary>
    private static IReadOnlyCollection<TeamModel> GetNotFullTeams(EventModel eventModel)
        => eventModel.Teams
            .Where(x => x.Members?.Length < eventModel.MinTeamMembers)
            .ToArray();
}
