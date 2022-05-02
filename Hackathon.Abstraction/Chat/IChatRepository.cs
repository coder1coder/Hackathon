using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;

namespace Hackathon.Abstraction.Chat;

public interface IChatRepository
{
    /// <summary>
    /// Добавить сообщение
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddMessage(ChatMessageEntity entity);

    /// <summary>
    /// Получить сообщения команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<BaseCollectionModel<TeamChatMessage>> GetTeamChatMessages(long teamId, int offset = 0, int limit = 300);
}