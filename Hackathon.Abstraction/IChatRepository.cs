using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;

namespace Hackathon.Abstraction;

public interface IChatRepository
{
    Task Add(ChatMessageEntity entity);

    Task<BaseCollectionModel<TeamChatMessage>> GetTeamChatMessages(long teamId, int offset = 0, int limit = 300);
}