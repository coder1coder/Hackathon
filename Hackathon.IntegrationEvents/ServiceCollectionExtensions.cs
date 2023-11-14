using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIntegrationEvents(this IServiceCollection services) => services
        .AddScoped<IIntegrationEventsHub<FriendshipChangedIntegrationEvent>,
            IntegrationEventsHub<FriendshipChangedIntegrationEvent>>()
        .AddScoped<IIntegrationEventsHub<EventStatusChangedIntegrationEvent>,
            IntegrationEventsHub<EventStatusChangedIntegrationEvent>>();
}
