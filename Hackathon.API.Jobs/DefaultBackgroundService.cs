using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hackathon.Jobs;

public abstract class DefaultBackgroundService<TJob>: BackgroundService where TJob: IJob
{
    private readonly ILogger<DefaultBackgroundService<TJob>> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private TimeSpan Delay { get; } = TimeSpan.FromMinutes(1);

    protected DefaultBackgroundService(
        ILogger<DefaultBackgroundService<TJob>> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var job = scope.ServiceProvider.GetRequiredService<TJob>();

            while (!stoppingToken.IsCancellationRequested)
            {
                await job.ExecuteAsync();
                await Task.Delay(Delay, stoppingToken);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Initiator}. Ошибка во время выполнения фоновой службы",
                typeof(DefaultBackgroundService<TJob>).Name);
        }

    }
}
