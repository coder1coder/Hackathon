using System;

namespace Hackathon.Jobs.Configuration.Events;

public class PastEventStatusUpdateJobSettings: IJobSettings
{
    public string? CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
