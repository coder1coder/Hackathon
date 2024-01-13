using Hackathon.Chats.Abstractions.Models;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.Abstractions.Repositories;
using MapsterMapper;

namespace Hackathon.Chats.DAL.Repositories;

public class TeamChatRepository: ChatRepository<TeamChatMessage>, ITeamChatRepository
{
    public TeamChatRepository(ChatsDbContext dbContext, IMapper mapper) : base(dbContext, mapper, ChatType.TeamChat)
    {
    }
}
