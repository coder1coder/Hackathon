using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Event.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using Hackathon.MessageQueue;
using Hackathon.MessageQueue.Messages;

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
            await _eventExistValidator.ValidateAndThrowAsync(eventId);
            return await _eventRepository.GetAsync(eventId);
        }

        public async Task<BaseCollectionModel<EventModel>> GetAsync(GetFilterModel<EventFilterModel> getFilterModel)
        {
            await _getFilterModelValidator.ValidateAndThrowAsync(getFilterModel);
            return await _eventRepository.GetAsync(getFilterModel);
        }

        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            await _eventExistValidator.ValidateAndThrowAsync(eventId);

            var eventModel = await _eventRepository.GetAsync(eventId);
            eventModel.Status = eventStatus;

            await _eventRepository.UpdateAsync(eventModel);

            var changeEventStatusMessage = eventModel.ChangeStatusMessages
                .FirstOrDefault(x => x.Status == eventStatus);

            if (changeEventStatusMessage != null)
                await _eventMessageHub.Publish(
                    TopicNames.EventChangeStatus,
                    new EventMessage(eventId, changeEventStatusMessage.Message));
        }

        public async Task DeleteAsync(long eventId)
        {
            await _eventExistValidator.ValidateAndThrowAsync(eventId);
            await _eventRepository.DeleteAsync(eventId);
        }
    }
}