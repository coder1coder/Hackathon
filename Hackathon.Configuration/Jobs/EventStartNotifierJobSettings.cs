using System;

namespace Hackathon.Configuration.Jobs;

public class EventStartNotifierJobSettings: IJobSettings
{
    public string CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
