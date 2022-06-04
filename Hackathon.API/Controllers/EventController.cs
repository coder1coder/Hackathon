using System;
using System.Threading.Tasks;
using Hackathon.Abstraction.Event;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers;

/// <summary>
/// События
/// </summary>
[SwaggerTag("События (мероприятия)")]
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
    [HttpPost]
    public async Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest)
    {
        var createEventModel = _mapper.Map<CreateEventModel>(createEventRequest);
        createEventModel.OwnerId = UserId;

        return new BaseCreateResponse
        {
            Id = await _eventService.CreateAsync(createEventModel),
        };
    }

    /// <summary>
    /// Обновить событие
    /// </summary>
    /// <param name="request"></param>
    [HttpPut]
    public async Task Update(UpdateEventRequest request)
    {
        var model = _mapper.Map<UpdateEventModel>(request);
        await _eventService.UpdateAsync(model);
    }

    /// <summary>
    /// Получить все события
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<EventListItem>))]
    public async Task<IActionResult> GetList([FromBody] GetListParameters<EventFilter> listRequest)
    {
        var getFilterModel = _mapper.Map<GetListParameters<EventFilter>>(listRequest);
        return Ok(await _eventService.GetListAsync(UserId, getFilterModel));
    }

    /// <summary>
    /// Получить событие по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventModel))]
    public async Task<EventModel> Get([FromRoute] long id)
        => await _eventService.GetAsync(id);

    ///<inheritdoc cref="IEventApi.Join"/>
    [HttpPost("{eventId:long}/join")]
    public async Task Join(long eventId) 
        => await _eventService.JoinAsync(eventId, UserId);
        
    [HttpPost("{eventId:long}/leave")]
    public async Task Leave(long eventId) 
        => await _eventService.LeaveAsync(eventId, UserId);

    [HttpPost("{eventId:long}/join/team")]
    public Task JoinTeam([FromRoute] long eventId, [FromQuery] long teamId)
        => throw new NotImplementedException();

    /// <summary>
    /// Задать статус события
    /// </summary>
    /// <param name="setStatusRequest"></param>
    [HttpPut(nameof(SetStatus))]
    public async Task SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        => await _eventService.SetStatusAsync(setStatusRequest.Id, setStatusRequest.Status);

    /// <summary>
    /// Полное удаление события
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id:long}")]
    public async Task Delete([FromRoute] long id)
        => await _eventService.DeleteAsync(id);
}