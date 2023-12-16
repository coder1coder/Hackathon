using System;

namespace Hackathon.Logbook.Abstraction.Models;

public class EventLogModel
{
    /// <summary>
    /// Идентификатор события аудита
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Тип события
    /// </summary>
    public EventLogType LogType { get; }

    /// <summary>
    /// Описание события
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Идентификатор пользователя инициатора события
    /// null если система
    /// </summary>
    public long? UserId { get; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Время события (UTC)
    /// </summary>
    public DateTime Timestamp { get; }

    public EventLogModel(EventLogType logType, string description = null, long? userId = null, string username = null)
    {
        Id = Guid.NewGuid();

        LogType = logType;

        Description = description ?? LogType switch
        {
            EventLogType.Created => "Создана запись",
            EventLogType.Updated => "Запись обновлена",
            EventLogType.Deleted => "Запись удалена",
            _ => null
        };

        Timestamp = DateTime.UtcNow;
        UserId = userId;
        Username = username;
    }
}
