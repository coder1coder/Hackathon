using System;
using Hackathon.Jobs.Configuration;
using Hackathon.Jobs.Configuration.Events;
using Hackathon.Jobs.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.AspNetCore;

namespace Hackathon.Jobs;

public static class ServiceCollectionExtensions
{

    /// <summary>
    /// Добавить сервисы для работы фоновых служб
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection RegisterApiJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(quartz=>
        {
            RegisterJob<EventStartNotifierJob, EventStartNotifierJobSettings>(EventStartNotifierJob.Key, configuration, quartz);
            RegisterJob<PastEventStatusUpdateJob, PastEventStatusUpdateJobSettings>(PastEventStatusUpdateJob.Key, configuration, quartz);
            RegisterJob<StartedEventStatusUpdateJob, StartedEventStatusUpdateJobSettings>(StartedEventStatusUpdateJob.Key, configuration, quartz);
            RegisterJob<UnusedFilesDeleteJob, UnusedFilesDeleteJobSettings>(UnusedFilesDeleteJob.Key, configuration, quartz);
        });

        services.AddQuartzServer(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }

    private static void RegisterJob<TJob, TJobSettings>(JobKey key, IConfiguration? configuration, IServiceCollectionQuartzConfigurator quartz)
        where TJob: BaseJob<TJob>
        where TJobSettings : class, IJobSettings, new()
    {
        var settings = configuration?.GetSection($"Jobs:{typeof(TJob).Name}")
            .Get<TJobSettings>() ?? new TJobSettings();

        quartz.AddJob<TJob>(x => x
            .WithIdentity(key)
        );

        var startAt = settings.StartAt.HasValue
            ? new DateTimeOffset(settings.StartAt.Value)
            : DateTimeOffset.UtcNow;

        switch (settings.ScheduleType)
        {
            case JobScheduleType.Interval:
                quartz.AddTrigger(x => x
                    .ForJob(key)
                    .WithIdentity(GetTriggerName(key.Name))
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
                    .ForJob(key)
                    .WithIdentity(GetTriggerName(key.Name))
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
