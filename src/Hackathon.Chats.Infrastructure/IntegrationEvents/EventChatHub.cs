using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.IntegrationEvents;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Abstraction.Events;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hackathon.Chats.Infrastructure.IntegrationEvents;

public class EventChatHub: Hub, IEventChatHub
{
    private readonly Func<long, string> _eventGroupNameResolver = eventId => $"event-{eventId}-chat";
    
    private readonly ILogger<EventChatHub> _logger;
    private readonly IChatConnectionsProvider _connectionsProvider;
    private readonly IEventRepository _eventRepository;

    public EventChatHub(
        ILogger<EventChatHub> logger, 
        IChatConnectionsProvider connectionsProvider, 
        IEventRepository eventRepository)
    {
        _logger = logger;
        _connectionsProvider = connectionsProvider;
        _eventRepository = eventRepository;
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

        var eventIds = await _eventRepository.GetUserActiveEventIds(userId);

        foreach (var userTeamId in eventIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _eventGroupNameResolver.Invoke(userTeamId));
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
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

        var eventIds = await _eventRepository.GetUserActiveEventIds(userId);

        foreach (var userTeamId in eventIds)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _eventGroupNameResolver.Invoke(userTeamId));
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendEventAsync(IEventChatIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        var topicName = integrationEvent.GetTopicName();

        if (topicName is null)
        {
            return;
        }

        await Clients
            .Group(_eventGroupNameResolver.Invoke(integrationEvent.EventId))
            .SendCoreAsync(topicName, [ integrationEvent ], cancellationToken);
    }

    public async Task AddToGroupAsync(long userId, long eventId, CancellationToken cancellationToken = default)
    {
        var connections = await _connectionsProvider.GetUserConnectionIdsAsync(userId, cancellationToken);

        if (!connections.Any())
        {
            _logger.LogWarning("Couldn't get connection id for user {UserId}", userId);
            return;
        }
        
        var groupName = _eventGroupNameResolver.Invoke(eventId);

        foreach (var connectionId in connections)
        {
            await Groups.AddToGroupAsync(connectionId, groupName, cancellationToken);
        }
    }

    public async Task RemoveFromGroupAsync(long userId, long eventId, CancellationToken cancellationToken = default)
    {
        var connections = await _connectionsProvider.GetUserConnectionIdsAsync(userId, cancellationToken);

        if (!connections.Any())
        {
            _logger.LogWarning("Couldn't get connection id for user {UserId}", userId);
            return;
        }
        
        var groupName = _eventGroupNameResolver.Invoke(eventId);

        foreach (var connectionId in connections)
        {
            await Groups.RemoveFromGroupAsync(connectionId, groupName, cancellationToken);
        }
    }
    
    public Task PublishAll(IIntegrationEvent integrationEvent)
    {
        throw new NotImplementedException();
    }
}
