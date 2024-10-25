using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Common.Messages.Teams;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Hackathon.Chats.Infrastructure.Consumers.Teams;

public class NewTeamMemberConsumer: IConsumer<NewTeamMemberMessage>
{
    private readonly ITeamChatHub _teamChatHub;
    private readonly ILogger<NewTeamMemberConsumer> _logger;

    public NewTeamMemberConsumer(
        ILogger<NewTeamMemberConsumer> logger, 
        ITeamChatHub teamChatHub)
    {
        _logger = logger;
        _teamChatHub = teamChatHub;
    }

    public async Task Consume(ConsumeContext<NewTeamMemberMessage> context)
    {
        _logger.LogInformation("Consumed new team member message. TeamId: {TeamId} MemberId: {MemberId}",
            context.Message.TeamId, context.Message.MemberId);
        
        await _teamChatHub.AddToGroupAsync(context.Message.MemberId, context.Message.TeamId, context.CancellationToken);
        
        _logger.LogInformation("User {UserId} was added to team hub with team {TeamId}",
            context.Message.MemberId, context.Message.TeamId);
    }
}
