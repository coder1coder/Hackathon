using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Models.Event;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public sealed class EventStatusChangedIntegrationEvent: IIntegrationEvent
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; }

    /// <summary>
    /// Статус события
    /// </summary>
    public EventStatus Status { get; }

    public EventStatusChangedIntegrationEvent(long eventId, EventStatus status)
    {
        EventId = eventId;
        Status = status;
    }
}
