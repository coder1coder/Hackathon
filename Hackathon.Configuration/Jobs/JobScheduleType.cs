namespace Hackathon.Configuration.Jobs;

/// <summary>
/// Тип выполнения джобы
/// </summary>
public enum JobScheduleType: byte
{
    /// <summary>
    /// В соответствии с выражением Cron
    /// </summary>
    Cron = 0,
    
    /// <summary>
    /// В соответствии с указанным интервалом
    /// </summary>
    Interval = 10
}
