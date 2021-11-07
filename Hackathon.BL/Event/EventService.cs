using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
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

            var eventEntity = _mapper.Map<EventEntity>(createEventModel);
            return await _eventRepository.CreateAsync(eventEntity);
        }
    }
}