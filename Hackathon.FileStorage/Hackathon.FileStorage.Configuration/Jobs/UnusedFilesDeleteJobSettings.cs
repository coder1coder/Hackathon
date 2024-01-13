using System;
using Hackathon.Configuration.Jobs;

namespace Hackathon.FileStorage.Configuration.Jobs;

public class UnusedFilesDeleteJobSettings: IJobSettings
{
    public string CronExpression { get; set; }
    public JobScheduleType ScheduleType { get; set; }
    public DateTime? StartAt { get; set; }
    public int? IntervalInDays { get; set; }
}
