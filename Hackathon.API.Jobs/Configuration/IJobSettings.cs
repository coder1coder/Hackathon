using System;

namespace Hackathon.Jobs.Configuration;

/// <summary>
/// Общие настройки для всех джоб
/// </summary>
public interface IJobSettings
{
    /// <summary>
    /// Выражение Cron
    /// </summary>
    /// <remarks>Если не указывать, то джоба не будет зарегистрирована</remarks>
    public string? IntervalCronExpression { get; set; }

    /// <summary>
    /// Дата и время начала работы
    /// </summary>
    /// <remarks>Если не указывать, то будет использована дата на момент регистрации</remarks>
    public DateTime? StartAt { get; set; }
}
