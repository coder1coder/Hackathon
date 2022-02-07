using System;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;
using ValidationException = Hackathon.Common.Exceptions.ValidationException;

namespace Hackathon.BL.Event
{
    public class EventService: IEventService
    {
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<GetListModel<EventFilterModel>> _getFilterModelValidator;
        private readonly ITeamService _teamService;

        public EventService(
            IValidator<CreateEventModel> createEventModelValidator,
            IValidator<GetListModel<EventFilterModel>> getFilterModelValidator,
            IEventRepository eventRepository,
            ITeamService teamService
            )
        {
            _createEventModelValidator = createEventModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            _eventRepository = eventRepository;
            _teamService = teamService;
        }

        /// <inheritdoc cref="IEventService.CreateAsync(CreateEventModel)"/>
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);
            return await _eventRepository.CreateAsync(createEventModel);
        }

        /// <inheritdoc cref="IEventService.GetAsync(long)"/>
        public async Task<EventModel> GetAsync(long eventId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);
            return eventModel ?? throw new EntityNotFoundException($"События с идентификатором {eventId} не существует");
        }

        /// <inheritdoc cref="IEventService.GetAsync(GetListModel{T})"/>
        public async Task<BaseCollectionModel<EventModel>> GetAsync(GetListModel<EventFilterModel> getListModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getListModel);
            return await _eventRepository.GetAsync(getListModel);
        }

        /// <inheritdoc cref="IEventService.SetStatusAsync(long, EventStatus)"/>
        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new EntityNotFoundException($"События с идентификатором {eventId} не существует");

            var canChangeStatus = (int) eventModel.Status == (int) eventStatus - 1;

            if (!canChangeStatus)
                throw new ValidationException(ErrorMessages.CantSetEventStatus);

            await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);
        }

        /// <inheritdoc cref="IEventService.JoinAsync(long, long)"/>
        public async Task JoinAsync(long eventId, long userId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            var notFullTeams = eventModel.TeamEvents.Select(x=>x.Team)
                .Where(x => x.Users.Count < eventModel.MinTeamMembers)
                .ToArray();

            long teamId;

            if (notFullTeams.Any())
            {
                teamId = notFullTeams.First().Id;
            }
            else
            {
                teamId = await _teamService.CreateAsync(new CreateTeamModel
                {
                    EventId = eventId,
                    Name = "Team-" + Guid.NewGuid().ToString()[..4]
                });
            }

            await _teamService.AddMemberAsync(new TeamAddMemberModel
            {
                TeamId = teamId,
                UserId = userId
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

        private async Task ChangeEventStatusAndPublishMessage(EventModel eventModel, EventStatus eventStatus)
        {
            await _eventRepository.SetStatusAsync(eventModel.Id, eventStatus);

            var changeEventStatusMessage = eventModel.ChangeEventStatusMessages
                .FirstOrDefault(x => x.Status == eventStatus);

            // if (changeEventStatusMessage != null)
            //     await _eventMessageHub.Publish(
            //         TopicNames.EventChangeStatus,
            //         new EventIntegrationEvent(eventModel.Id, changeEventStatusMessage.Message));
        }
    }
}