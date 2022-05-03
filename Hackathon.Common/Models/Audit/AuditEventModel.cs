using System;

namespace Hackathon.Common.Models.Audit;

public class AuditEventModel
{
    /// <summary>
    /// Идентификатор события аудита
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Тип события
    /// </summary>
    public AuditEventType Type { get; } 
    
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
    /// Время события (UTC)
    /// </summary>
    public DateTime Timestamp { get; }

    public AuditEventModel(AuditEventType type, string description = null, long? userId = null)
    {
        Type = type;
        
        Description = description ?? Type switch
        {
            AuditEventType.Created => "Создана запись",
            AuditEventType.Updated => "Запись обновлена",
            AuditEventType.Deleted => "Запись удалена",
            _ => null
        };
        
        Timestamp = DateTime.UtcNow;
        UserId = userId;
    }
}