using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface IChatRepository
{
    /// <summary>
    /// Добавить сообщение
    /// </summary>
    /// <param name="chatMessage"></param>
    /// <returns></returns>
    Task AddMessage<TChatMessage>(TChatMessage chatMessage) where TChatMessage : IChatMessage;

    /// <summary>
    /// Получить сообщения команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<BaseCollection<TeamChatMessage>> GetTeamChatMessages(long teamId, int offset = 0, int limit = 300);
}
