using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.BL.Event.Validators;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Extensions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Audit;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Notification;
using Hackathon.Common.Models.Team;
using Hackathon.Entities;
using MassTransit;
using ValidationException = Hackathon.Common.Exceptions.ValidationException;

namespace Hackathon.BL.Event
{
    /// <summary>
    /// События
    /// </summary>
    public class EventService: IEventService
    {
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IValidator<UpdateEventModel> _updateEventModelValidator;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;
        private readonly IValidator<GetListParameters<EventFilter>> _getFilterModelValidator;
        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationService;
        private readonly IBus _messageBus;

        public EventService(
            IValidator<CreateEventModel> createEventModelValidator,
            IValidator<UpdateEventModel> updateEventModelValidator,
            IValidator<GetListParameters<EventFilter>> getFilterModelValidator,
            IEventRepository eventRepository,
            ITeamService teamService, 
            IUserRepository userRepository, 
            INotificationService notificationService,
            IBus messageBus)
        {
            _createEventModelValidator = createEventModelValidator;
            _updateEventModelValidator = updateEventModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            _eventRepository = eventRepository;
            _teamService = teamService;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _messageBus = messageBus;
        }

        /// <inheritdoc cref="IEventService.CreateAsync(CreateEventModel)"/>
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);
            var eventId = await _eventRepository.CreateAsync(createEventModel);
            
            await _messageBus.Publish(new AuditEventModel(
                AuditEventType.Created,
                $"Создано новое событие с идентификатором '{eventId}'",
                createEventModel.OwnerId
            ));
            
            return eventId;
        }

        /// <inheritdoc cref="IEventService.UpdateAsync(UpdateEventModel)"/>
        public async Task UpdateAsync(UpdateEventModel updateEventModel)
        {
            await _updateEventModelValidator.ValidateAndThrowAsync(updateEventModel);
            await _eventRepository.UpdateAsync(updateEventModel);
        }

        /// <inheritdoc cref="IEventService.GetAsync(long)"/>
        public async Task<EventModel> GetAsync(long eventId)
            => await _eventRepository.GetAsync(eventId);

        /// <inheritdoc cref="IEventService.GetListAsync"/>
        public async Task<BaseCollection<EventListItem>> GetListAsync(long userId, GetListParameters<EventFilter> getListParameters)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListParameters);
            var events = await _eventRepository.GetListAsync(userId, getListParameters);

            return new BaseCollection<EventListItem>
            {
                TotalCount = events.TotalCount,
                Items = events.Items?
                    .Select(x => x.ToListItem())
                    .ToArray()
            };
        }

        /// <inheritdoc cref="IEventService.SetStatusAsync(long, EventStatus)"/>
        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new EntityNotFoundException($"События с идентификатором {eventId} не существует");

            var (isValid, errorMessage) = await new ChangeEventStatusValidator().ValidateAsync(eventModel, eventStatus);

            if (!isValid)
                throw new ValidationException(errorMessage);
            
            await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);
        }

        /// <inheritdoc cref="IEventService.JoinAsync(long, long)"/>
        public async Task JoinAsync(long eventId, long userId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel.Status != EventStatus.Published)
                throw new ValidationException("Нельзя присоединиться к событию");
            
            var notFullTeams = eventModel
                .Teams
                .Where(x => x.Members?.Count < eventModel.MinTeamMembers)
                .ToArray();

             var teamId = notFullTeams.Any()
                    ? notFullTeams.First().Id
                    : eventModel.IsCreateTeamsAutomatically
                        ? await _teamService.CreateAsync(new CreateTeamModel
                        {
                            EventId = eventId,
                            Name = $"Team-{eventId}-{Guid.NewGuid().ToString()[..4]}"
                        })
                        : throw new ValidationException("Невозможно присоединиться к событию");

            await _teamService.AddMemberAsync(new TeamMemberModel
            {
                TeamId = teamId,
                MemberId = userId
            });
        }

        /// <summary>
        /// Покинуть событие
        /// </summary>
        /// <param name="eventId">Идентификатор события</param>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <exception cref="Hackathon.Common.Exceptions.ValidationException"></exception>
        public async Task LeaveAsync(long eventId, long userId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new ValidationException("События не существует");

            if (eventModel.Status != EventStatus.Published)
                throw new ValidationException("Нельзя покидать событие, когда оно уже начато");

            var userExists = await _userRepository.ExistAsync(userId);

            if (!userExists)
                throw new ValidationException("Пользователь не существует");

            var userTeam = GetTeamContainsMember(eventModel, userId);
            if (userTeam == null)
                throw new ValidationException("Пользователь не состоит в событии");

            if (userTeam.OwnerId.HasValue)
                throw new ValidationException("Нельзя покинуть событие, если вступили командой");

            await _teamService.RemoveMemberAsync(new TeamMemberModel
            {
                TeamId = userTeam.Id,
                MemberId = userId
            });
        }

        /// <inheritdoc cref="IEventService.DeleteAsync(long)"/>
        public async Task DeleteAsync(long eventId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new EntityNotFoundException($"События с идентификатором {eventId} не существует");

            await _eventRepository.DeleteAsync(eventId);
        }

        /// <inheritdoc cref="IEventService.GetByExpression(Expression{Func{EventEntity, bool}})"/>
        public async Task<EventModel[]> GetByExpression(Expression<Func<EventEntity, bool>> expression)
            => await _eventRepository.GetByExpression(expression);

        /// <summary>
        /// Меняет статус события и отправляет сообщение в шину с уведомлением участников события
        /// </summary>
        /// <param name="eventModel"></param>
        /// <param name="eventStatus"></param>
        private async Task ChangeEventStatusAndPublishMessage(EventModel eventModel, EventStatus eventStatus)
        {
            await _eventRepository.SetStatusAsync(eventModel.Id, eventStatus);

            var changeEventStatusMessage = eventModel.ChangeEventStatusMessages
                                               .FirstOrDefault(x => x.Status == eventStatus)
                                               ?.Message
                                           ?? eventStatus.ToDefaultChangedEventStatusMessage(eventModel.Name);

            var usersIds = eventModel.Teams
                .SelectMany(x => x.Members?.Select(z => z.Id))
                .ToArray();

            if (usersIds.Any())
            {
                var notificationModels = usersIds.Select(x =>
                    NotificationFactory.InfoNotification(changeEventStatusMessage, x));
                
                await _notificationService.PushMany(notificationModels);
            }
        }

        private static TeamModel GetTeamContainsMember(EventModel eventModel, long userId)
            => eventModel.Teams
                .FirstOrDefault(x => x.Members.Any(s=>s.Id == userId));
    }
}