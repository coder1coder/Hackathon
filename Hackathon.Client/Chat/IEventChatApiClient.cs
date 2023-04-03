using Hackathon.Common.Models.Chat.Event;
using Refit;

namespace Hackathon.Client.Chat;

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
    /// <returns></returns>
    [Post(BaseRoute + "/{eventId}/list")]
    public Task GetListAsync(long eventId, [Query] int offset = 0, [Query] int limit = 300);
}
