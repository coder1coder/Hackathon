using Hackathon.Common.Models.EventLog;

namespace Hackathon.Entities;

/// <summary>
/// Событие аудита
/// </summary>
public class EventLogEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// Тип события
    /// </summary>
    public EventLogType Type { get; set; }

    /// <summary>
    /// Идентификатор пользователя инициатора события
    /// null если система
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Описание события
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Время события (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; }
}
