namespace Hackathon.Common.Models.Event;

/// <summary>
/// Этап события
/// </summary>
public class EventStage
{
    /// <summary>
    /// Наименование
    /// </summary>
    public EventStatus Status { get; set; }
    
    /// <summary>
    ///  Продолжительность (мин)
    /// </summary>
    public int Duration { get; set; }
}