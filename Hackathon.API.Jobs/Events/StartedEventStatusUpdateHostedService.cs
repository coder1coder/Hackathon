using Hackathon.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

public class StartedEventStatusUpdateHostedService: DefaultBackgroundService<StartedEventStatusUpdateJob>
{
    public StartedEventStatusUpdateHostedService(
        ILogger<DefaultBackgroundService<StartedEventStatusUpdateJob>> logger,
        IServiceScopeFactory serviceScopeFactory)
        : base(logger, serviceScopeFactory)
    {
        Delay = JobsConstants.StartedEventUpdateStatusJobFrequency;
    }
}
