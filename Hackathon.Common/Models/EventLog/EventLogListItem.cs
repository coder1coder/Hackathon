using System;

namespace Hackathon.Common.Models.EventLog;

/// <summary>
/// Модель списочного представления записи журнала событий
/// </summary>
public class EventLogListItem
{
    /// <summary>
    /// Идентификатор события аудита
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Тип события
    /// </summary>
    public EventLogType LogType { get; set; }

    /// <summary>
    /// Описание события
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор пользователя инициатора события
    /// null если система
    /// </summary>
    public long? UserId { get; set; }

    public string? UserName { get; set; }

    /// <summary>
    /// Время события (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; }
}
