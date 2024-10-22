using System;

namespace Hackathon.Configuration.Jobs;

/// <summary>
/// Настройки джобы уведомлений о начале мероприятия
/// </summary>
public class EventStartNotifierJobSettings: IJobSettings
{
    public string CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
