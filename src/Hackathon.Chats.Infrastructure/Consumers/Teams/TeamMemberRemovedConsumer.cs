using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Messages.Teams;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hackathon.Chats.Infrastructure.Consumers.Teams;

public class TeamMemberRemovedConsumer: IConsumer<TeamMemberRemovedMessage>
{
    private readonly ILogger<TeamMemberRemovedConsumer> _logger;
    private readonly ITeamChatHub _teamChatHub;

    public TeamMemberRemovedConsumer(ILogger<TeamMemberRemovedConsumer> logger, ITeamChatHub teamChatHub)
    {
        _logger = logger;
        _teamChatHub = teamChatHub;
    }

    public async Task Consume(ConsumeContext<TeamMemberRemovedMessage> context)
    {
        _logger.LogInformation("Consumed {Message}. TeamId: {TeamId} MemberId: {MemberId}",
            nameof(TeamMemberRemovedMessage), context.Message.TeamId, context.Message.MemberId);
        
        await _teamChatHub.RemoveFromGroupAsync(context.Message.MemberId, context.Message.TeamId, context.CancellationToken);
        
        _logger.LogInformation("User {UserId} was removed from team hub group with team {TeamId}",
            context.Message.MemberId, context.Message.TeamId);
    }
}
