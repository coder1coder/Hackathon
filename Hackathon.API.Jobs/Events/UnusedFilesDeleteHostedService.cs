using Hackathon.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hackathon.Jobs.Events;

public class UnusedFilesDeleteHostedService : DefaultBackgroundService<UnusedFilesDeleteJob>
{
    public UnusedFilesDeleteHostedService(
        ILogger<DefaultBackgroundService<UnusedFilesDeleteJob>> logger, 
        IServiceScopeFactory serviceScopeFactory) : base(logger, serviceScopeFactory)
    {
        Delay = JobsConstants.UnusedFilesDeleteJobFrequency;
    }
}

