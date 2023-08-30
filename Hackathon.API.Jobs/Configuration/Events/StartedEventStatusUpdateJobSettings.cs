using System;

namespace Hackathon.Jobs.Configuration.Events;

public class StartedEventStatusUpdateJobSettings: IJobSettings
{
    public string? IntervalCronExpression { get; set; }
    public DateTime? StartAt { get; set; }
}
