using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Event.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using MapsterMapper;

namespace Hackathon.BL.Event
{
    public class EventService: IEventService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<GetFilterModel<EventFilterModel>> _getFilterModelValidator;
        private readonly EventExistValidator _eventExistValidator;

        public EventService(IMapper mapper,
            IValidator<CreateEventModel> createEventModelValidator,
            EventExistValidator eventExistValidator,
            IValidator<GetFilterModel<EventFilterModel>> getFilterModelValidator,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _createEventModelValidator = createEventModelValidator;
            _getFilterModelValidator = getFilterModelValidator;
            _eventExistValidator = eventExistValidator;
            _eventRepository = eventRepository;
        }
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);

            var eventModel = _mapper.Map<EventModel>(createEventModel);
            return await _eventRepository.CreateAsync(eventModel);
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
        }

        public async Task DeleteAsync(long eventId)
        {
            await _eventExistValidator.ValidateAndThrowAsync(eventId);
            await _eventRepository.DeleteAsync(eventId);
        }
    }
}