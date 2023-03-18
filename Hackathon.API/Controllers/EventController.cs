using Hackathon.Common.Abstraction.Event;
using System;
using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(BaseCreateResponse))]
    public async Task<IActionResult> Create(CreateEventRequest createEventRequest)
    {
        var createEventModel = _mapper.Map<CreateEventRequest, EventCreateParameters>(createEventRequest);
        createEventModel.OwnerId = UserId;

        var createResult = await _eventService.CreateAsync(createEventModel);
        if (!createResult.IsSuccess)
            return await GetResult(() => Task.FromResult(createResult));

        return Ok(new BaseCreateResponse
        {
            Id = createResult.Data
        });
    }

    /// <summary>
    /// Обновить событие
    /// </summary>
    /// <param name="request"></param>
    [HttpPut]
    public Task<IActionResult> Update(UpdateEventRequest request)
        => GetResult(() => _eventService.UpdateAsync(_mapper.Map<EventUpdateParameters>(request)));

    /// <summary>
    /// Получить все события
    /// </summary>
    /// <returns></returns>
    [HttpPost("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<EventListItem>))]
    public Task<IActionResult> GetList([FromBody] GetListParameters<EventFilter> listRequest)
        => GetResult(() => _eventService.GetListAsync(UserId, _mapper.Map<GetListParameters<EventFilter>>(listRequest)));

    /// <summary>
    /// Получить событие по идентификатору
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventModel))]
    public Task<IActionResult> Get([FromRoute] long id)
        => GetResult(() => _eventService.GetAsync(id));

    [HttpPost("{eventId:long}/join")]
    public Task<IActionResult> Join(long eventId)
        => GetResult(() => _eventService.JoinAsync(eventId, UserId));

    [HttpPost("{eventId:long}/leave")]
    public Task<IActionResult> Leave(long eventId)
        => GetResult(() => _eventService.LeaveAsync(eventId, UserId));

    [HttpPost("{eventId:long}/join/team")]
    public Task<IActionResult> JoinTeam([FromRoute] long eventId, [FromQuery] long teamId)
        => throw new NotImplementedException();

    /// <summary>
    /// Задать статус события
    /// </summary>
    /// <param name="setStatusRequest"></param>
    [HttpPut(nameof(SetStatus))]
    public Task<IActionResult> SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        => GetResult(() => _eventService.SetStatusAsync(setStatusRequest.Id, setStatusRequest.Status));

    /// <summary>
    /// Переключить событие на следующий этап
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    [HttpPost("{eventId:long}/stages/next")]
    public Task<IActionResult> GoNextStage([FromRoute] long eventId)
        => GetResult(() => _eventService.GoNextStage(UserId, eventId));

    /// <summary>
    /// Удалить событие
    /// </summary>
    /// <param name="id"></param>
    [HttpDelete("{id:long}")]
    public Task<IActionResult> Delete([FromRoute] long id)
        => GetResult(() => _eventService.DeleteAsync(id));
}
