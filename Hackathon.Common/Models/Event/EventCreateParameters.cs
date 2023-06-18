using Hackathon.Common.Models.Event.Agreement;

namespace Hackathon.Common.Models.Event;

/// <summary>
/// Модель события для создания
/// </summary>
public class EventCreateParameters: BaseEventParameters
{
    /// <summary>
    /// Кто создал событие
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Соглашение об участии в мероприятии
    /// </summary>
    public EventAgreementCreateParameters Agreement { get; set; }
}
