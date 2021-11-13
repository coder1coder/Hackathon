using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models.Event;
using MapsterMapper;

namespace Hackathon.BL.Event
{
    public class EventService: IEventService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateEventModel> _createEventModelValidator;
        private readonly IEventRepository _eventRepository;

        public EventService(IMapper mapper, IValidator<CreateEventModel> createEventModelValidator,
            IEventRepository eventRepository)
        {
            _mapper = mapper;
            _createEventModelValidator = createEventModelValidator;
            _eventRepository = eventRepository;
        }
        public async Task<long> CreateAsync(CreateEventModel createEventModel)
        {
            await _createEventModelValidator.ValidateAndThrowAsync(createEventModel);

            var eventModel = _mapper.Map<EventModel>(createEventModel);
            return await _eventRepository.CreateAsync(eventModel);
        }

        public async Task SetStatusAsync(long eventId, EventStatus eventStatus)
        {
            var eventModel = await GetEventThrowIfNotExist(eventId);
            eventModel.Status = eventStatus;
            await _eventRepository.UpdateAsync(eventModel);
        }

        public async Task DeleteAsync(long eventId)
        {
            await GetEventThrowIfNotExist(eventId);
            await _eventRepository.DeleteAsync(eventId);
        }

        private async Task<EventModel> GetEventThrowIfNotExist(long eventId)
        {
            var eventModel = await _eventRepository.GetAsync(eventId);

            if (eventModel == null)
                throw new ServiceException("Событие с таким идентификатором не найдено");

            return eventModel;
        }
    }
}