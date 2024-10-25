using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Abstraction.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hackathon.Chats.Infrastructure.IntegrationEvents;

[Authorize]
public class TeamChatHub: Hub, ITeamChatHub
{
    private readonly Func<long, string> _teamGroupNameResolver = teamId => $"team-{teamId}-chat";
    
    private readonly IHubContext<TeamChatHub> _hubContext;
    private readonly ILogger<TeamChatHub> _logger;
    private readonly ITeamRepository _teamRepository;
    private readonly IChatConnectionsProvider _connectionsProvider;
    
    public TeamChatHub(
        IHubContext<TeamChatHub> hubContext, 
        ILogger<TeamChatHub> logger, 
        ITeamRepository teamRepository, 
        IChatConnectionsProvider connectionsProvider)
    {
        _hubContext = hubContext;
        _logger = logger;
        _teamRepository = teamRepository;
        _connectionsProvider = connectionsProvider;
    }
    
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User {UserId} connected with connection id {ConnectionId}",
            Context.UserIdentifier,
            Context.ConnectionId);

        if (!long.TryParse(Context.UserIdentifier, out var userId))
        {
            _logger.LogWarning("Someone has been connected to hub without valid identity");
            return;
        }

        await _connectionsProvider.SaveUserConnectionIdAsync(userId, Context.ConnectionId);

        var userTeamIds = await _teamRepository.GetUserTeamIdsAsync(userId);

        foreach (var userTeamId in userTeamIds)
        {
            await _hubContext.Groups.AddToGroupAsync(Context.ConnectionId, _teamGroupNameResolver.Invoke(userTeamId));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("User {UserId} disconnected with connection id {ConnectionId}",
            Context.UserIdentifier,
            Context.ConnectionId);
        
        if (!long.TryParse(Context.UserIdentifier, out var userId))
        {
            _logger.LogWarning("Someone has been disconnected from hub without valid identity");
            return;
        }

        await _connectionsProvider.RemoveUserConnectionIdAsync(userId, Context.ConnectionId);

        var userTeamIds = await _teamRepository.GetUserTeamIdsAsync(userId);

        foreach (var userTeamId in userTeamIds)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(Context.ConnectionId, _teamGroupNameResolver.Invoke(userTeamId));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendEventAsync(ITeamChatIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var topicName = ResolveTopicName(integrationEvent);

        if (topicName is null)
        {
            return;
        }

        await _hubContext.Clients
            .Group(_teamGroupNameResolver.Invoke(integrationEvent.TeamId))
            .SendCoreAsync(topicName, [ integrationEvent ], cancellationToken);
    }

    public async Task AddToGroupAsync(long userId, long teamId, CancellationToken cancellationToken = default)
    {
        var connections = await _connectionsProvider.GetUserConnectionIdsAsync(userId, cancellationToken);

        if (!connections.Any())
        {
            _logger.LogWarning("Couldn't get connection id for user {UserId}", userId);
            return;
        }
        
        var teamGroupName = _teamGroupNameResolver.Invoke(teamId);

        foreach (var connectionId in connections)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, teamGroupName, cancellationToken);
        }
    }

    public async Task RemoveFromGroupAsync(long userId, long teamId, CancellationToken cancellationToken = default)
    {
        var connections = await _connectionsProvider.GetUserConnectionIdsAsync(userId, cancellationToken);

        if (!connections.Any())
        {
            _logger.LogWarning("Couldn't get connection id for user {UserId}", userId);
            return;
        }
        
        var teamGroupName = _teamGroupNameResolver.Invoke(teamId);

        foreach (var connectionId in connections)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, teamGroupName, cancellationToken);
        }
    }

    private static string? ResolveTopicName(ITeamChatIntegrationEvent integrationEvent)
    {
        return integrationEvent switch
        {
            TeamChatNewMessageIntegrationEvent => ChatsTopicNames.TeamChatNewMessage,
            _ => null
        };
    }
    
    public Task PublishAll(IIntegrationEvent integrationEvent)
    {
        throw new NotImplementedException();
    }
}
