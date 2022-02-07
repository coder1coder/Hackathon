using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Notification
{
    public static class Dependencies
    {
        public static IServiceCollection AddNotificationDependencies(this IServiceCollection services)
        {
            return services;
        }
    }
}