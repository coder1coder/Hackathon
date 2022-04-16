using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;

namespace Hackathon.Abstraction;

public interface IChatService
{
    Task SendMessage(ICreateChatMessage createChatMessage);
    Task<BaseCollectionModel<TeamChatMessage>> GetTeamMessages(long teamId, int offset = 0, int limit = 300);
}