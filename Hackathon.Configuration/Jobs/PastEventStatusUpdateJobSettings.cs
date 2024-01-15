using System;

namespace Hackathon.Configuration.Jobs;

/// <summary>
/// Джоба выставления статуса завершенного мероприятия по истечении времени
/// </summary>
public class PastEventStatusUpdateJobSettings: IJobSettings
{
    public string CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
