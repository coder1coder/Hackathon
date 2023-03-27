using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIntegrationEvents(this IServiceCollection services) => services
        .AddScoped<IMessageHub<NotificationChangedIntegrationEvent>,
            IntegrationEventHub<NotificationChangedIntegrationEvent>>()
        .AddScoped<IMessageHub<ChatMessageChangedIntegrationEvent>,
            IntegrationEventHub<ChatMessageChangedIntegrationEvent>>()
        .AddScoped<IMessageHub<FriendshipChangedIntegrationEvent>,
            IntegrationEventHub<FriendshipChangedIntegrationEvent>>()
        .AddScoped<IMessageHub<EventStatusChangedIntegrationEvent>,
            IntegrationEventHub<EventStatusChangedIntegrationEvent>>();
}
