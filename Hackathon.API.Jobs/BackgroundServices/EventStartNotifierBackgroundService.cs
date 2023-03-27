using System;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.BackgroundServices;

public class EventStartNotifierBackgroundService: BackgroundService
{
    private readonly ILogger<EventStartNotifierBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public EventStartNotifierBackgroundService(
        ILogger<EventStartNotifierBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService<IEventStartNotifierJob>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await job.ExecuteAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Initiator}. Ошибка во время выполнения фоновой службы", nameof(EventStartNotifierBackgroundService));
        }

    }
}
