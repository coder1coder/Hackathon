namespace Hackathon.Common.Models.Event;

/// <summary>
/// Задача мероприятия
/// </summary>
public class EventTaskItem
{
    /// <summary>
    /// Заголовок задачи
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Порядок отображения
    /// </summary>
    public int Order { get; set; }
}
