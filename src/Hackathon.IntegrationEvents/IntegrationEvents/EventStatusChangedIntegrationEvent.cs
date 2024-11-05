using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Models.Event;
using Hackathon.IntegrationEvents.Topics;

namespace Hackathon.IntegrationEvents.IntegrationEvents;

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

    public string GetTopicName() => EventsTopicNames.EventStatusChanged;
}
