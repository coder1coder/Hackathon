using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

public sealed class EventStartNotifierHostedService: DefaultBackgroundService<EventStartNotifierJob>
{
    public EventStartNotifierHostedService(
        ILogger<EventStartNotifierHostedService> logger,
        IServiceScopeFactory factory)
        : base(logger, factory)
    {
    }
}
