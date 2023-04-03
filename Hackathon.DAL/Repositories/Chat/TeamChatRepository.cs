using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Team;
using StackExchange.Redis;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hackathon.DAL.Repositories.Chat;

public class TeamChatRepository: ChatRepository<TeamChatMessage>, ITeamChatRepository
{
    protected override Expression<Func<TeamChatMessage, long>> EntityIdentityKey => x => x.TeamId;

    public TeamChatRepository(IConnectionMultiplexer connectionMultiplexer) : base(connectionMultiplexer)
    {
    }

    public Task<BaseCollection<TeamChatMessage>> GetMessagesAsync(long teamId, int offset = 0, int limit = 300)
        => GetMessagesByKeyAsync(teamId.ToString(), offset, limit);
}
