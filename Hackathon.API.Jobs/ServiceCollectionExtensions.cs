using Hackathon.Jobs.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiJobs(this IServiceCollection services) => services
        .AddScoped<EventStartNotifierJob>()
        .AddHostedService<EventStartNotifierHostedService>()

        .AddScoped<PastEventStatusUpdateJob>()
        .AddHostedService<PastEventStatusUpdateHostedService>()
    ;
}
