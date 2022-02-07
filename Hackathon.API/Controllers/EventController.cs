using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    /// <summary>
    /// События
    /// </summary>
    public class EventController: BaseController, IEventApi
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
        [HttpPost]
        public async Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest)
        {
            var createEventModel = _mapper.Map<CreateEventModel>(createEventRequest);
            createEventModel.UserId = UserId;

            return new BaseCreateResponse
            {
                Id = await _eventService.CreateAsync(createEventModel),
            };
        }

        /// <summary>
        /// Получить все события
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<EventModel>))]
        public async Task<IActionResult> Get([FromQuery] GetListRequest<EventFilterModel> listRequest)
        {
            var getFilterModel = _mapper.Map<GetListModel<EventFilterModel>>(listRequest);
            var collectionModel = await _eventService.GetAsync(getFilterModel);
            return Ok(_mapper.Map<BaseCollectionResponse<EventModel>>(collectionModel));
        }

        /// <summary>
        /// Получить событие по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventModel))]
        public async Task<EventModel> Get([FromRoute] long id)
        {
            return await _eventService.GetAsync(id);
        }

        [HttpPost("{eventId:long}/Join")]
        public async Task Join(long eventId)
        {
            await _eventService.JoinAsync(eventId, UserId);
        }

        /// <summary>
        /// Задать статус события
        /// </summary>
        /// <param name="setStatusRequest"></param>
        [HttpPut(nameof(SetStatus))]
        public async Task SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        {
            await _eventService.SetStatusAsync(setStatusRequest.Id, setStatusRequest.Status);
        }

        /// <summary>
        /// Полное удаление события
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id:long}")]
        public async Task Delete([FromRoute] long id)
        {
            await _eventService.DeleteAsync(id);
        }

    }
}