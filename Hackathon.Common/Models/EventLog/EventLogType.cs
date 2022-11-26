namespace Hackathon.Common.Models.EventLog;

/// <summary>
/// Тип произошедшего события
/// </summary>
public enum EventLogType
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
