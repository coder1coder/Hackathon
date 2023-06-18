using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Event;
using Hackathon.Common.Models.Chat.Team;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.API.Controllers.Chat;

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
    /// Получить список сообщений чата команды
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
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
