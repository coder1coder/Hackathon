using System;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Contracts.Events;
using Hackathon.API.Module;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Event;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers.Events;

/// <summary>
/// События
/// </summary>
[SwaggerTag("События (мероприятия)")]
public class EventController : BaseController
{
    private readonly IEventService _eventService;
    private readonly IMapper _mapper;

    /// <summary>
    /// События
    /// </summary>
    public EventController(IEventService eventService, IMapper mapper)
    {
        _eventService = eventService;
        _mapper = mapper;
    }

    /// <summary>
    /// Создание нового события
    /// </summary>
    /// <param name="createEventRequest"></param>
    [HttpPost]
    [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(BaseCreateResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public  Task<IActionResult> Create(CreateEventRequest createEventRequest)
    {
        var createEventParameters = _mapper.Map<CreateEventRequest, EventCreateParameters>(createEventRequest);
        return GetResult(() => _eventService.CreateAsync(AuthorizedUserId, createEventParameters),
            result => new BaseCreateResponse
            {
                Id = result
            });
    }

    /// <summary>
    /// Обновить событие
    /// </summary>
    /// <param name="request"></param>
    [HttpPut]
    public Task<IActionResult> Update(UpdateEventRequest request)
        => GetResult(() => _eventService.UpdateAsync(AuthorizedUserId, _mapper.Map<EventUpdateParameters>(request)));

    /// <summary>
    /// Получить все события
    /// </summary>
    [HttpPost("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollection<EventListItem>))]
    public Task<IActionResult> GetList([FromBody] GetListParameters<EventFilter> listRequest)
        => GetResult(() => _eventService.GetListAsync(AuthorizedUserId, _mapper.Map<GetListParameters<EventFilter>>(listRequest)));

    /// <summary>
    /// Получить событие по идентификатору
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    [HttpGet("{eventId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventModel))]
    public Task<IActionResult> Get([FromRoute] long eventId)
        => GetResult(() => _eventService.GetAsync(AuthorizedUserId, eventId));

    /// <summary>
    /// Присоединиться к мероприятию
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    [HttpPost("{eventId:long}/join")]
    public Task<IActionResult> Join(long eventId)
        => GetResult(() => _eventService.JoinAsync(eventId, AuthorizedUserId));

    /// <summary>
    /// Получить соглашение мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    [HttpGet("{eventId:long}/agreement")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EventAgreementModel))]
    public Task<IActionResult> GetAgreement(long eventId)
        => GetResult(() => _eventService.GetAgreementAsync(eventId));

    /// <summary>
    /// Принять соглашение мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    [HttpPost("{eventId:long}/agreement/accept")]
    public Task<IActionResult> AcceptAgreement(long eventId)
        => GetResult(() => _eventService.AcceptAgreementAsync(AuthorizedUserId, eventId));

    /// <summary>
    /// Покинуть мероприятие
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    [HttpPost("{eventId:long}/leave")]
    public Task<IActionResult> Leave(long eventId)
        => GetResult(() => _eventService.LeaveAsync(eventId, AuthorizedUserId));

    /// <summary>
    /// Задать статус события
    /// </summary>
    /// <param name="setStatusRequest"></param>
    [HttpPut(nameof(SetStatus))]
    public Task<IActionResult> SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        => GetResult(() => _eventService.SetStatusAsync(AuthorizedUserId, setStatusRequest.Id, setStatusRequest.Status));

    /// <summary>
    /// Переключить событие на следующий этап
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    [HttpPost("{eventId:long}/stages/next")]
    public Task<IActionResult> GoNextStage([FromRoute] long eventId)
        => GetResult(() => _eventService.GoNextStage(AuthorizedUserId, eventId));

    /// <summary>
    /// Удалить событие
    /// </summary>
    /// <param name="eventId"></param>
    [HttpDelete("{eventId:long}")]
    public Task<IActionResult> Delete([FromRoute] long eventId)
        => GetResult(() => _eventService.DeleteAsync(eventId, AuthorizedUserId));

    /// <summary>
    /// Загрузить изображение события
    /// </summary>
    /// <param name="file">Файл изображения</param>
    [HttpPost("image/upload")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public Task<IActionResult> UploadEventImage(IFormFile file)
        => GetResult(() => _eventService.UploadEventImageAsync(file));
}
