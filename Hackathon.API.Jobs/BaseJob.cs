using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Jobs;

public abstract class BaseJob<TJob>: IJob
{
    public static JobKey Key => new(typeof(TJob).Name);

    protected abstract Task DoWork(IJobExecutionContext context);

    private readonly ILogger<TJob> _logger;

    protected BaseJob(ILogger<TJob> logger)
    {
       _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await DoWork(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Source} Ошибка во время работы джобы: {Error}",
                typeof(TJob).Name,
                e.Message);
        }
    }

}
