using Hackathon.Common.Models.Audit;

namespace Hackathon.Abstraction.Entities;

/// <summary>
/// Событие аудита
/// </summary>
public class AuditEventEntity
{
    public Guid Id { get; set; }
        
    /// <summary>
    /// Тип события
    /// </summary>
    public AuditEventType Type { get; set; }
    
    /// <summary>
    /// Идентификатор пользователя инициатора события
    /// null если система
    /// </summary>
    public long? UserId { get; set; }
    
    /// <summary>
    /// Имя пользователя инициатора события
    /// </summary>
    public string? UserName { get; set; }
    
    /// <summary>
    /// Описание события
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// Время события (UTC)
    /// </summary>
    public DateTime Timestamp { get; set; }
}