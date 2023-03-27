using Hackathon.Common.Abstraction.Jobs;
using Hackathon.Jobs.BackgroundServices;
using Hackathon.Jobs.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiJobs(this IServiceCollection services) => services
        .AddScoped<IEventStartNotifierJob, EventStartNotifierJob>()
        .AddHostedService<EventStartNotifierBackgroundService>();
}
