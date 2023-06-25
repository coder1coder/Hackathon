using BackendTools.Common.Models;
using FluentValidation;
using Hackathon.BL.Validation.Event;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Abstraction.Event;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Notification;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventLog;
using Hackathon.Common.Models.Notification;
using Hackathon.Common.Models.Team;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.FileStorage;
using Microsoft.AspNetCore.Http;

namespace Hackathon.BL.Event;

/// <summary>
/// События
/// </summary>
public class EventService : IEventService
{
    private readonly IValidator<EventCreateParameters> _createEventModelValidator;
    private readonly IValidator<EventUpdateParameters> _updateEventModelValidator;
    private readonly IEventRepository _eventRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IFileStorageRepository _fileStorageRepository;
    private readonly IUserRepository _userRepository;
    private readonly IValidator<Common.Models.GetListParameters<EventFilter>> _getFilterModelValidator;
    private readonly ITeamService _teamService;
    private readonly INotificationService _notificationService;
    private readonly IBus _messageBus;
    private readonly IMessageHub<EventStatusChangedIntegrationEvent> _integrationEventHub;
    private readonly IEventAgreementRepository _eventAgreementRepository;

    public EventService(
        IValidator<EventCreateParameters> createEventModelValidator,
        IValidator<EventUpdateParameters> updateEventModelValidator,
        IValidator<Common.Models.GetListParameters<EventFilter>> getFilterModelValidator,
        IEventRepository eventRepository,
        ITeamService teamService,
        IUserRepository userRepository,
        INotificationService notificationService,
        IBus messageBus,
        IMessageHub<EventStatusChangedIntegrationEvent> integrationEventHub,
        IFileStorageService fileStorageService,
        IFileStorageRepository fileStorageRepository,
        IEventAgreementRepository eventAgreementRepository)
    {
        _createEventModelValidator = createEventModelValidator;
        _updateEventModelValidator = updateEventModelValidator;
        _getFilterModelValidator = getFilterModelValidator;
        _eventRepository = eventRepository;
        _teamService = teamService;
        _userRepository = userRepository;
        _notificationService = notificationService;
        _messageBus = messageBus;
        _integrationEventHub = integrationEventHub;
        _fileStorageService = fileStorageService;
        _fileStorageRepository = fileStorageRepository;
        _eventAgreementRepository = eventAgreementRepository;
    }

    public async Task<Result<long>> CreateAsync(EventCreateParameters eventCreateParameters)
    {
        await _createEventModelValidator.ValidateAndThrowAsync(eventCreateParameters);

        if (!await IsSuccessRemoveStorageFileFlag(eventCreateParameters.ImageId))
        {
            eventCreateParameters.ImageId = null;
            await CreateEvent(eventCreateParameters);
            return Result<long>.NotValid(EventErrorMessages.EventSavedButImageDoesNotExists);
        }

        var eventId = await CreateEvent(eventCreateParameters);

        return Result<long>.FromValue(eventId);
    }

    public async Task<Result> UpdateAsync(EventUpdateParameters eventUpdateParameters)
    {
        await _updateEventModelValidator.ValidateAndThrowAsync(eventUpdateParameters);

        var eventModel = await _eventRepository.GetAsync(eventUpdateParameters.Id);

        if (eventModel is null)
            return Result.NotFound(EventErrorMessages.EventDoesNotExists);

        if (eventModel.ImageId.GetValueOrDefault() != eventUpdateParameters.ImageId.GetValueOrDefault())
        {
            //Помечаем старый файл, если такой существует, как удаленный
            if (eventModel.ImageId.HasValue)
                await _fileStorageRepository.UpdateFlagIsDeleted(eventModel.ImageId.Value, true);

            if (!await IsSuccessRemoveStorageFileFlag(eventUpdateParameters.ImageId))
            {
                eventUpdateParameters.ImageId = null;
                await _eventRepository.UpdateAsync(eventUpdateParameters);
                return Result.NotValid(EventErrorMessages.EventSavedButImageDoesNotExists);
            }     
        }

        await _eventRepository.UpdateAsync(eventUpdateParameters);

        return Result.Success;
    }

    public async Task<Result<EventModel>> GetAsync(long eventId)
    {
        var @event = await _eventRepository.GetAsync(eventId);
        return Result<EventModel>.FromValue(@event);
    }

    public async Task<Result<BaseCollection<EventListItem>>> GetListAsync(long userId, Common.Models.GetListParameters<EventFilter> getListParameters)
    {
        await _getFilterModelValidator.ValidateAndThrowAsync(getListParameters);
        var events = await _eventRepository.GetListAsync(userId, getListParameters);

        return Result<BaseCollection<EventListItem>>.FromValue(new BaseCollection<EventListItem>
        {
            TotalCount = events.TotalCount,
            Items = events.Items?
                .ToArray()
        });
    }

    public async Task<Result> SetStatusAsync(long eventId, EventStatus eventStatus, bool skipValidation = false)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel == null)
            return Result.NotFound(EventErrorMessages.EventDoesNotExists);

        if (!skipValidation)
        {
            var (isValid, errorMessage) = ChangeEventStatusValidator.ValidateAsync(eventModel, eventStatus);

            if (!isValid)
                return Result.NotValid(errorMessage);
        }

        if (eventStatus == EventStatus.Started)
        {
            var initialStageId = eventModel.Stages.OrderBy(x => x.Order).FirstOrDefault()?.Id ?? default;
            await _eventRepository.SetCurrentStageId(eventId, initialStageId);
        }

        await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);

        return Result.Success;
    }

    public async Task<Result> JoinAsync(long eventId, long userId)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel.Agreement?.RequiresConfirmation == true
            && eventModel.Agreement?.Users?.Any(x=>x.Id == userId) != true)
            return Result.NotValid(EventMessages.AgreementShouldBeSigned);

        if (eventModel.Status != EventStatus.Published)
            return Result.NotValid(EventMessages.CantAttachToEvent);

        var notFullTeams = eventModel.GetNotFullTeams();

        if (notFullTeams.Count == 0 && !eventModel.IsCreateTeamsAutomatically)
            return Result.NotValid(EventMessages.CantAttachToEvent);

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
                return Result.FromErrors(createTeamResult.Errors);

            teamId = createTeamResult.Data;
        }

        var addMemberResult = await _teamService.AddMemberAsync(new TeamMemberModel
        {
            TeamId = teamId,
            MemberId = userId
        });

        return !addMemberResult.IsSuccess
            ? addMemberResult
            : Result.Success;
    }

    /// <summary>
    /// Покинуть событие
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="userId">Идентификатор пользователя</param>
    public async Task<Result> LeaveAsync(long eventId, long userId)
    {
        var eventModel = await _eventRepository.GetAsync(eventId);

        if (eventModel is null)
            return Result.NotValid(EventErrorMessages.EventDoesNotExists);

        if (eventModel.Status != EventStatus.Published)
            return Result.NotValid(EventMessages.CantLeaveEventWhenItsAlreadyStarted);

        var userExists = await _userRepository.ExistsAsync(userId);

        if (!userExists)
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        var userTeam = GetTeamContainsMember(eventModel, userId);
        if (userTeam == null)
            return Result.NotValid(EventMessages.UserIsNotInEvent);

        if (userTeam.OwnerId.HasValue)
            return Result.NotValid(EventMessages.CantLeaveEventIfYouEnteredAsTeam);

        await _teamService.RemoveMemberAsync(new TeamMemberModel
        {
            TeamId = userTeam.Id,
            MemberId = userId
        });

        return Result.Success;
    }

    public async Task<Result> DeleteAsync(long eventId)
    {
        var eventExists = await _eventRepository.ExistsAsync(eventId);

        if (!eventExists)
            Result.NotValid(EventErrorMessages.EventDoesNotExists);

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
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        var eventModel = await _eventRepository.GetAsync(eventId);
        if (eventModel is null)
            return Result.NotValid(EventErrorMessages.EventDoesNotExists);

        if (eventModel.Owner?.Id != userId)
            return Result.Forbidden("Только владелец события может переключить этап");

        var notAvailableStatusesForGoingNextStage = new[] {EventStatus.Draft, EventStatus.Published, EventStatus.Finished};

        if (notAvailableStatusesForGoingNextStage.Contains(eventModel.Status))
            return Result.NotValid("Текущий статус события не позволяет перейти к следующему этапу");

        var orderedStages = eventModel.Stages.OrderBy(x => x.Order).ToList();
        var currentStageIndex = orderedStages.FindIndex(x => x.Id == eventModel.CurrentStageId);

        if (currentStageIndex < 0)
            return Result.Internal("Невозможно определить текущее событие");

        if (orderedStages.Count - 1 == currentStageIndex)
            return Result.NotValid("Текущий этап события является последним");

        var nextStageId = orderedStages[++currentStageIndex].Id;
        await _eventRepository.SetCurrentStageId(eventId, nextStageId);

        return Result.Success;
    }

    /// <summary>
    /// Загрузить изображение события
    /// </summary>
    /// <param name="file">Файл http запроса</param>
    /// <returns></returns>
    public async Task<Result<Guid>> UploadEventImageAsync(IFormFile file)
    {
        if (file is null)
            return Result<Guid>.NotValid(EventMessages.EventFileImageIsNotBeEmpty);

        await using var stream = file.OpenReadStream();

        var uploadResult = await _fileStorageService.UploadAsync(stream, Bucket.Events, file.FileName);

        //Обращаемся к загруженному файлу и ставим флаг - как удаляемый
        await _fileStorageRepository.UpdateFlagIsDeleted(uploadResult.Id, true);

        return Result<Guid>.FromValue(uploadResult.Id);
    }

    public async Task<Result<EventAgreementModel>> GetAgreementAsync(long eventId)
    {
        var agreement = await _eventAgreementRepository.GetByEventId(eventId);
        return agreement is null
            ? Result<EventAgreementModel>.NotValid(EventErrorMessages.AgreementDoesNotExists)
            : Result<EventAgreementModel>.FromValue(agreement);
    }

    public async Task<Result> AcceptAgreementAsync(long authorizedUserId, long eventId)
    {
        var user = await _userRepository.GetAsync(authorizedUserId);
        if (user is null)
            return Result.NotValid(UserErrorMessages.UserDoesNotExists);

        var agreement = await _eventAgreementRepository.GetByEventId(eventId);
        if (agreement is null)
            return Result.NotValid(EventErrorMessages.AgreementDoesNotExists);

        if (agreement.Event is null)
            return Result.NotValid(EventErrorMessages.EventDoesNotExists);

        if (agreement.Event.Status is not EventStatus.Published)
            return Result.NotValid("Мероприятие должно быть опубликовано");

        if (agreement.Users.Any(x=>x.Id == authorizedUserId))
            return Result.Success;

        if (!agreement.RequiresConfirmation)
            return Result.Success;

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

        await _integrationEventHub.Publish(TopicNames.EventStatusChanged, new EventStatusChangedIntegrationEvent(eventModel.Id, eventStatus));
        await NotifyEventMembers(eventModel, eventStatus);
    }

    private async Task NotifyEventMembers(EventModel eventModel, EventStatus eventStatus)
    {
        var changeEventStatusMessage = eventModel.ChangeEventStatusMessages
                                           .FirstOrDefault(x => x.Status == eventStatus)
                                           ?.Message
                                       ?? eventStatus.ToDefaultChangedEventStatusMessage(eventModel.Name);

        var usersIds = eventModel.Teams
            .SelectMany(x => x.Members?.Select(z => z.Id))
            .ToArray();

        if (!usersIds.Any())
            return;

        var notificationModels = usersIds.Select(x =>
            NotificationFactory.InfoNotification(changeEventStatusMessage, x));

        await _notificationService.PushManyAsync(notificationModels);
    }

    private static TeamModel GetTeamContainsMember(EventModel eventModel, long userId)
        => eventModel.Teams
            .FirstOrDefault(x =>
                x.Members?.Any(s=>s.Id == userId) == true);

    /// <summary>
    /// Сформировать наименование для команды создаваемой автоматически
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    private static string GenerateAutoCreatedTeamName(long eventId)
        => $"Team-{eventId}-{Guid.NewGuid().ToString()[..4]}";

    private async Task<bool> IsSuccessRemoveStorageFileFlag(Guid? fileId)
    {
        if (fileId.HasValue)
        {
            var fileModel = await _fileStorageRepository.GetAsync(fileId.Value);

            if (fileModel is not null)
            {
                //Снимаем ранее установленный флаг, чтобы не удалить фотографию события
                await _fileStorageRepository.UpdateFlagIsDeleted(fileModel.Id, false);
            }
            else
                return false;
        }

        return true;
    }

    private async Task<long> CreateEvent(EventCreateParameters eventCreateParameters)
    {
        var eventId = await _eventRepository.CreateAsync(eventCreateParameters);
        await _messageBus.Publish(new EventLogModel(
            EventLogType.Created,
            $"Создано новое событие с идентификатором '{eventId}'",
            eventCreateParameters.OwnerId
        ));

        return eventId;
    }
}
