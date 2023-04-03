using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Event;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface IEventChatService
{
    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    /// <param name="ownerId">Идентификатор автора сообщения</param>
    /// <param name="newEventChatMessage">Сообщение</param>
    /// <returns></returns>
    Task<Result> SendAsync(long ownerId, NewEventChatMessage newEventChatMessage);

    /// <summary>
    /// Получить список сообщений
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<Result<BaseCollection<EventChatMessage>>> GetListAsync(long eventId, int offset = 0, int limit = 300);
}
