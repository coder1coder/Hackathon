using Hackathon.Common;
using Hackathon.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Hackathon.Jobs.Events;

public class UnusedFilesDeleteHostedService : DefaultBackgroundService<UnusedFilesDeleteJob>
{
    public UnusedFilesDeleteHostedService(
        ILogger<DefaultBackgroundService<UnusedFilesDeleteJob>> logger, 
        IServiceScopeFactory serviceScopeFactory,
        IOptions<FileSettings> fileSettings) : base(logger, serviceScopeFactory)
    {
        Delay = TimeSpan.FromHours(fileSettings?.Value?.UnusedFilesDeleteJobFrequencyInHours ??
            JobsConstants.UnusedFilesDeleteJobFrequency);
    }
}

