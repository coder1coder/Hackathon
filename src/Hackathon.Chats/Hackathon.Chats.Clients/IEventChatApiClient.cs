using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.Models.Events;
using Refit;

namespace Hackathon.Chats.Clients;

public interface IEventChatApiClient
{
    private const string BaseRoute = "/api/chat/event";

    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    /// <param name="newEventChatMessage"></param>
    [Post(BaseRoute + "/send")]
    Task SendAsync([Body] NewEventChatMessage newEventChatMessage);

    /// <summary>
    /// Получить список сообщений чата команды
    /// </summary>
    /// <param name="eventId">Идентификатор мероприятия</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    [Post(BaseRoute + "/{eventId}/list")]
    Task GetListAsync(long eventId, [Query] int offset = 0, [Query] int limit = 300);
}
