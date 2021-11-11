using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpPost(nameof(Create))]
        public async Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest)
        {
            var createEventModel = _mapper.Map<CreateEventModel>(createEventRequest);
            var eventId = await _eventService.CreateAsync(createEventModel);
            return new BaseCreateResponse
            {
                Id = eventId,
            };
        }

    }
}