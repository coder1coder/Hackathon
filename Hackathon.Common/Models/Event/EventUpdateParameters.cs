using Hackathon.Common.Models.Event.Agreement;

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

    /// <summary>
    /// Соглашение об участии в мероприятии
    /// </summary>
    public EventAgreementUpdateParameters Agreement { get; set; }
}
