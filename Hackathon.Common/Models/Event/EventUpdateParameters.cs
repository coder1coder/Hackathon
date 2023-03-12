namespace Hackathon.Common.Models.Event;

/// <summary>
/// Модель события для обновления
/// </summary>
public class EventUpdateParameters : BaseEventParameters
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long Id { get; set; }
}
