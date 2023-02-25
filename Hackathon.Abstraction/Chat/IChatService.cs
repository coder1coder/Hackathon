using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;

namespace Hackathon.Abstraction.Chat;

public interface IChatService
{
    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    /// <param name="createChatMessage">Сообщение</param>
    /// <returns></returns>
    Task<Result> SendMessage(ICreateChatMessage createChatMessage);

    /// <summary>
    /// Получить сообщения команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<Result<BaseCollection<TeamChatMessage>>> GetTeamMessages(long teamId, int offset = 0, int limit = 300);
}
