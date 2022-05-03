namespace Hackathon.Common.Models.Audit;

/// <summary>
/// Тип произошедшего события
/// </summary>
public enum AuditEventType
{
    /// <summary>
    /// Значение по умолчанию
    /// </summary>
    Default = 0,
    
    /// <summary>
    /// Создана запись
    /// </summary>
    Created = 1,
    
    /// <summary>
    /// Запись обновлена
    /// </summary>
    Updated = 2,
    
    /// <summary>
    /// Запись удалена
    /// </summary>
    Deleted = 3,
}