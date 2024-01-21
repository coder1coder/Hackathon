using System;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;

namespace Hackathon.Chats.Abstractions.Repositories;

/// <summary>
/// Репозиторий сообщений чата
/// </summary>
/// <typeparam name="TChatMessage">Тип сообщений</typeparam>
public interface IChatRepository<TChatMessage> where TChatMessage: class
{
    /// <summary>
    /// Добавить сообщение
    /// </summary>
    /// <param name="chatMessage"></param>
    /// <returns></returns>
    Task<Guid> AddMessageAsync(TChatMessage chatMessage);

    /// <summary>
    /// Получить список сообщений
    /// </summary>
    /// <param name="chatId">Идентификатор чата</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<BaseCollection<TChatMessage>> GetMessagesAsync(long chatId, int offset = 0, int limit = 300);

    /// <summary>
    /// Получить сообщение чата
    /// </summary>
    /// <param name="messageId">Идентификатор сообщения</param>
    /// <returns></returns>
    Task<Result<TChatMessage>> GetMessageAsync(Guid messageId);
}
