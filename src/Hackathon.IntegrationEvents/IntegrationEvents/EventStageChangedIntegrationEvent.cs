using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.IntegrationEvents.IntegrationEvents;

public sealed class EventStageChangedIntegrationEvent: IIntegrationEvent
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; }
    
    /// <summary>
    /// Идентификатор этапа мероприятия
    /// </summary>
    public long EventStageId { get; }
    
    public EventStageChangedIntegrationEvent(long eventId, long eventStageId)
    {
        EventId = eventId;
        EventStageId = eventStageId;
    }
}
