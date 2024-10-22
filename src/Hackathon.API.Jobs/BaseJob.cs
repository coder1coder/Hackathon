using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Hackathon.Jobs;

public abstract class BaseBackgroundJob<TJob>: IBackgroundJob, IJob
{
    public abstract Task DoWork();

    private readonly ILogger<TJob> _logger;

    protected BaseBackgroundJob(ILogger<TJob> logger)
    {
       _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await DoWork();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Source} Ошибка во время работы джобы: {Error}",
                typeof(TJob).Name,
                e.Message);
        }
    }

}
