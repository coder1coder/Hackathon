using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface ITeamChatService
{
    /// <summary>
    /// Отправить сообщение в чат
    /// </summary>
    /// <param name="ownerId">Идентификатор автора сообщения</param>
    /// <param name="newTeamChatMessage">Сообщение</param>
    /// <returns></returns>
    Task<Result> SendAsync(long ownerId, NewTeamChatMessage newTeamChatMessage);

    /// <summary>
    /// Получить сообщения команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<Result<BaseCollection<TeamChatMessage>>> GetListAsync(long teamId, int offset = 0, int limit = 300);
}
