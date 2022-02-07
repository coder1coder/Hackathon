using Hackathon.Notification;
using Hackathon.Notification.IntegrationEvent;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.API
{
    public static class Dependencies
    {
        public static IServiceCollection AddApiDependencies(this IServiceCollection services)
        {
            return services
                // .AddScoped<IMapper, Mapper>()
                .AddScoped<IMessageHub<NotificationPublishedIntegrationEvent>, IntegrationEventHub<NotificationPublishedIntegrationEvent>>();
        }
    }
}