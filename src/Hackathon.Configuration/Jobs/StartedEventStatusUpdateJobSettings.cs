using System;

namespace Hackathon.Configuration.Jobs;

/// <summary>
/// Джоба автоматического выставления статуса начала мероприятия
/// </summary>
public class StartedEventStatusUpdateJobSettings: IJobSettings
{
    public string CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
