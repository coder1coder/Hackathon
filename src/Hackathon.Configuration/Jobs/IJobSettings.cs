using System;

namespace Hackathon.Configuration.Jobs;

/// <summary>
/// Общие настройки для всех джоб
/// </summary>
public interface IJobSettings
{
    /// <summary>
    /// Выражение Cron
    /// </summary>
    /// <remarks>Если не указывать, то джоба не будет зарегистрирована</remarks>
    public string CronExpression { get; set; }

    /// <summary>
    /// Тип планирования
    /// </summary>
    public JobScheduleType ScheduleType { get; set; }

    /// <summary>
    /// Дата и время начала работы
    /// </summary>
    /// <remarks>Если не указывать, то будет использована дата на момент регистрации</remarks>
    public DateTime? StartAt { get; set; }

    /// <summary>
    /// Интервал повторений в днях
    /// </summary>
    public int? IntervalInDays { get; set; }
}
