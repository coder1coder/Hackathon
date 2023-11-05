using System;
using Hackathon.Configuration.Jobs;
using Hackathon.FileStorage.Configuration.Jobs;
using Hackathon.FileStorage.Jobs.Jobs;
using Hackathon.Jobs;
using Hackathon.Jobs.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace Hackathon.API.Extensions;

public static class JobsServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы для работы фоновых служб
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartz =>
        {
            AddJob<EventStartNotifierJob>(quartz, GetJobSettings<EventStartNotifierJob, EventStartNotifierJobSettings>(configuration));
            AddJob<PastEventStatusUpdateJob>(quartz, GetJobSettings<PastEventStatusUpdateJob, PastEventStatusUpdateJobSettings>(configuration));
            AddJob<StartedEventStatusUpdateJob>(quartz, GetJobSettings<StartedEventStatusUpdateJob, StartedEventStatusUpdateJobSettings>(configuration));
            AddJob<UnusedFilesDeleteJob>(quartz, GetJobSettings<UnusedFilesDeleteJobSettings, UnusedFilesDeleteJobSettings>(configuration));
        });
        
        return services.AddQuartzServer(x => x.WaitForJobsToComplete = true);
    }
    
    public static void AddJob<TJob>(IServiceCollectionQuartzConfigurator quartz, IJobSettings jobSettings) where TJob: IBackgroundJob
        => AddJob(typeof(TJob), jobSettings, quartz);

    private static TJobSettings GetJobSettings<TJob, TJobSettings>(IConfiguration configuration) where TJobSettings: IJobSettings, new()
        => configuration.GetSection($"Jobs:{typeof(TJob).Name}").Get<TJobSettings>() ?? new TJobSettings();

    private static void AddJob(Type jobType, IJobSettings settings, IServiceCollectionQuartzConfigurator quartz)
    {
        var jobName = jobType.Name;
        quartz.AddJob(jobType, JobKey.Create(jobName), x => x.WithIdentity(jobName));

        var startAt = settings.StartAt.HasValue
            ? new DateTimeOffset(settings.StartAt.Value)
            : DateTimeOffset.UtcNow;
        
         switch (settings.ScheduleType)
        {
            case JobScheduleType.Interval:
                quartz.AddTrigger(x => x
                    .ForJob(jobName)
                    .WithIdentity(GetTriggerName(jobName))
                    .StartAt(startAt)
                    .WithCalendarIntervalSchedule(s=>s
                        .WithIntervalInDays(settings.IntervalInDays.GetValueOrDefault())
                        .WithMisfireHandlingInstructionDoNothing())
                );

                break;

            case JobScheduleType.Cron:
            default:

                if (string.IsNullOrWhiteSpace(settings.CronExpression)
                    || !CronExpression.IsValidExpression(settings.CronExpression))
                    return;

                quartz.AddTrigger(x => x
                    .ForJob(jobName)
                    .WithIdentity(GetTriggerName(jobName))
                    .StartAt(startAt)
                    .WithCronSchedule(settings.CronExpression, builder =>
                        builder.WithMisfireHandlingInstructionDoNothing()));
                break;
        }
    }

    /// <summary>
    /// Получить наименование триггера
    /// </summary>
    /// <param name="jobName">Наименование джобы</param>
    /// <returns></returns>
    private static string GetTriggerName(string jobName) => $"{jobName}-trigger";
}
