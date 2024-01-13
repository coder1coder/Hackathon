using System;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.Chats.Module.Controllers;

[Route("api/chat/event")]
[SwaggerTag("Чат мероприятия")]
public class EventChatController: BaseController
{
    private readonly IEventChatService _eventChatService;

    public EventChatController(IEventChatService eventChatService)
    {
        _eventChatService = eventChatService;
    }

    /// <summary>
    /// Отправить сообщение в чат мероприятия
    /// </summary>
    /// <param name="newEventChatMessage">Сообщение</param>
    /// <returns></returns>
    [HttpPost("send")]
    public Task<IActionResult> SendEventChatMessage([FromBody] NewEventChatMessage newEventChatMessage)
        => GetResult(() => _eventChatService.SendAsync(AuthorizedUserId, newEventChatMessage));

    /// <summary>
    /// Получить сообщение чата мероприятия по идентификатору
    /// </summary>
    /// <returns></returns>
    [HttpGet("messages/{messageId:guid}")]
    public Task<IActionResult> GetEventChatMessage(Guid messageId)
        => GetResult(() => _eventChatService.GetAsync(AuthorizedUserId, messageId));

    /// <summary>
    /// Получить список сообщений чата мероприятия
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [HttpPost("{eventId:long}/list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseCollection<TeamChatMessage>))]
    public Task<IActionResult> GetList(
        [FromRoute] long eventId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
        => GetResult(() => _eventChatService.GetListAsync(eventId, offset, limit));
}
