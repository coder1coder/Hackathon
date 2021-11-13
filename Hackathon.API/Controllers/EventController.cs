using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class EventController: BaseController
    {
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventController(IEventService eventService, IMapper mapper)
        {
            _eventService = eventService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание нового события
        /// </summary>
        /// <param name="createEventRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest)
        {
            var createEventModel = _mapper.Map<CreateEventModel>(createEventRequest);
            var eventId = await _eventService.CreateAsync(createEventModel);
            return new BaseCreateResponse
            {
                Id = eventId,
            };
        }

        /// <summary>
        /// Задать статус события
        /// </summary>
        /// <param name="setStatusRequest"></param>
        [Authorize]
        [HttpPut(nameof(SetStatus))]
        public async Task SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        {
            await _eventService.SetStatusAsync(setStatusRequest.Id, setStatusRequest.Status);
        }

        /// <summary>
        /// Полное удаление события
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task Delete([FromRoute] long id)
        {
            await _eventService.DeleteAsync(id);
        }

    }
}