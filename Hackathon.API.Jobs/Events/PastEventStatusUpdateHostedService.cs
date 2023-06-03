using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

public sealed class PastEventStatusUpdateHostedService: DefaultBackgroundService<PastEventStatusUpdateJob>
{
    public PastEventStatusUpdateHostedService(
        ILogger<PastEventStatusUpdateHostedService> logger,
        IServiceScopeFactory factory)
        : base(logger, factory)
    {
    }
}
