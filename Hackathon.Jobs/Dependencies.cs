using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Jobs
{
    public static class Dependencies
    {
        public static IServiceCollection AddJobsDependencies(this IServiceCollection services)
        {
            return services.AddScoped<IChangeEventStatusJob, ChangeEventStatusJob>();
        }
    }
}