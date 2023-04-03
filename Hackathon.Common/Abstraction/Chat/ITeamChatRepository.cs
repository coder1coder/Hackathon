using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface ITeamChatRepository: IChatRepository<TeamChatMessage>
{
    Task<BaseCollection<TeamChatMessage>> GetMessagesAsync(long teamId, int offset = 0, int limit = 300);
}
