using System;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;

namespace Hackathon.Chats.Abstractions.Services;

public interface IChatService<in TNewChatMessage, TChatMessage> where TChatMessage: class
{
    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    /// <param name="ownerId">Идентификатор автора сообщения</param>
    /// <param name="newMessage">Сообщение</param>
    Task<Result> SendAsync(long ownerId, TNewChatMessage newMessage);

    /// <summary>
    /// Получить сообщение чата
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор пользователя</param>
    /// <param name="messageId">Идентификатор сообщения</param>
    Task<Result<TChatMessage>> GetAsync(long authorizedUserId, Guid messageId);

    /// <summary>
    /// Получить список сообщений
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    Task<Result<BaseCollection<TChatMessage>>> GetListAsync(long chatId, int offset = 0, int limit = 300);
}
