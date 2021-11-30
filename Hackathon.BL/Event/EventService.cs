using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Event.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.MessageQueue;
using Hackathon.MessageQueue.Messages;
using ValidationException = Hackathon.Common.Exceptions.ValidationException;

namespace Hackathon.BL.Event
{
    public class EventService: IEventService
    {
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<GetFilterModel<EventFilterModel>> _getFilterModelValidator;
        private readonly EventExistValidator _eventExistValidator;
        private readonly IMessageHub<EventMessage> _eventMessageHub;

        public EventService(
            IValidator<CreateEventModel> createEventModelValidator,
            EventExistValidator eventExistValidator,
            IValidator<GetFilterModel<EventFilterModel>> getFilterModelValidator,
            IEventRepository eventRepository,
            IMessageHub<EventMessage> eventMessageHub
            )
        {
            _createEventModelValidator = createEventModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            _eventExistValidator = eventExistValidator;
            _eventRepository = eventRepository;
            _eventMessageHub = eventMessageHub;
        }
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);
            return await _eventRepository.CreateAsync(createEventModel);
        }

        public async Task<EventModel> GetAsync(long eventId)
        {
            await _eventExistValidator.ValidateAndThrowAsync(new []{ eventId });
            return await _eventRepository.GetAsync(eventId);
        }

        public async Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getFilterModel);
            return await _eventRepository.GetAsync(getFilterModel);
        }

        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            await _eventExistValidator.ValidateAndThrowAsync(new []{ eventId });

            var eventModel = await _eventRepository.GetAsync(eventId);

            var canChangeStatus = (int) eventModel.Status == (int) eventStatus - 1;

            if (!canChangeStatus)
                throw new ValidationException(ErrorMessages.CantSetEventStatus);

            await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);
        }

        public async Task SetStatusAsync(long[] eventsIds, EventStatus eventStatus)
        {
            await _eventExistValidator.ValidateAndThrowAsync(eventsIds);

            var eventModels = await _eventRepository.GetAsync(new GetFilterModel<EventFilterModel>
            {
                Filter = new EventFilterModel
                {
                    Ids = eventsIds,
                    Status = eventStatus - 1
                }
            });

            var canChangeStatus = eventModels.TotalCount == eventsIds.Length;

            if (!canChangeStatus)
                throw new ValidationException(ErrorMessages.CantSetEventsStatus);

            foreach (var eventModel in eventModels.Items)
                await ChangeEventStatusAndPublishMessage(eventModel, eventStatus);
        }

        public async Task DeleteAsync(long eventId)
        {
            await _eventExistValidator.ValidateAndThrowAsync(new []{ eventId });
            await _eventRepository.DeleteAsync(eventId);
        }

        private async Task ChangeEventStatusAndPublishMessage(EventModel eventModel, EventStatus eventStatus)
        {
            eventModel.Status = eventStatus;
            await _eventRepository.UpdateAsync(new []{ eventModel });

            var changeEventStatusMessage = eventModel.ChangeEventStatusMessages
                .FirstOrDefault(x => x.Status == eventStatus);

            if (changeEventStatusMessage != null)
                await _eventMessageHub.Publish(
                    TopicNames.EventChangeStatus,
                    new EventMessage(eventModel.Id, changeEventStatusMessage.Message));
        }
    }
}