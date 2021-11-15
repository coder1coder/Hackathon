using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    /// <summary>
    /// События
    /// </summary>
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
        /// Получить все события
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<BaseCollectionResponse<EventModel>> Get([FromQuery] GetFilterRequest<EventFilterModel> filterRequest)
        {
            var getFilterModel = _mapper.Map<GetFilterModel<EventFilterModel>>(filterRequest);
            var collectionModel = await _eventService.GetAsync(getFilterModel);
            return _mapper.Map<BaseCollectionResponse<EventModel>>(collectionModel);
        }

        /// <summary>
        /// Получить событие по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<EventModel> Get([FromRoute] long id)
        {
            return await _eventService.GetAsync(id);
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
        /// Установить дату начала регистрации события
        /// </summary>
        /// <param name="setStartRegistrationRequest"></param>
        [Authorize]
        [HttpPut(nameof(SetStartRegistration))]
        public async Task SetStartRegistration(SetStartRegistrationRequest setStartRegistrationRequest)
        {
            await _eventService.SetStartRegistrationAsync(setStartRegistrationRequest.Id, setStartRegistrationRequest.StartRegistration);
        }
        
        /// <summary>
        /// Задать минимальное количество участников для события
        /// </summary>
        /// <param name="setMinTeamMembersRequest"></param>
        [Authorize]
        [HttpPut(nameof(SetMinTeamMembers))]
        public async Task SetMinTeamMembers(SetMinTeamMembersRequest setMinTeamMembersRequest)
        {
            await _eventService.SetMinTeamMembersAsync(setMinTeamMembersRequest.Id, setMinTeamMembersRequest.MinTeamMembers);
        }

        /// <summary>
        /// Задать максимальное количество участников для события
        /// </summary>
        /// <param name="setMaxTeamMembersRequest"></param>
        [Authorize]
        [HttpPut(nameof(SetMaxTeamMembers))]
        public async Task SetMaxTeamMembers(SetMaxTeamMembersRequest setMaxTeamMembersRequest)
        {
            await _eventService.SetMaxTeamMembersAsync(setMaxTeamMembersRequest.Id, setMaxTeamMembersRequest.MaxTeamMembers);
        }
        
        /// <summary>
        /// Полное удаление события
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete("{id:long}")]
        public async Task Delete([FromRoute] long id)
        {
            await _eventService.DeleteAsync(id);
        }

    }
}