using System;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.Abstraction.Jobs;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.BackgroundServices;

public class EventStartNotifierBackgroundService: BackgroundService
{
    private readonly ILogger<EventStartNotifierBackgroundService> _logger;
    private readonly IEventStartNotifierJob _job;

    public EventStartNotifierBackgroundService(
        ILogger<EventStartNotifierBackgroundService> logger, 
        IEventStartNotifierJob job)
    {
        _logger = logger;
        _job = job;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _job.Execute();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Ошибка во время выполнения фоновой службы");
        }
        
    }
}