using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents.Hubs;
using Hackathon.IntegrationEvents.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterIntegrationEvents(this IServiceCollection services, bool isDevelopmentEnvironment)
    {
        services
            .AddScoped<IIntegrationEventsHub<FriendshipChangedIntegrationEvent>, IntegrationEventsHub<FriendshipChangedIntegrationEvent>>()
            .AddScoped<IEventChangesIntegrationEventsHub, EventChangesIntegrationEventsHub>()
            .AddSingleton<IUserIdProvider, IntegrationEventsUserIdProvider>();

        services.AddSignalR(x =>
            x.EnableDetailedErrors = isDevelopmentEnvironment);

        return services;
    }
}
