using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Models.Chat;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public sealed class ChatMessageChangedIntegrationEvent: IIntegrationEvent
{
    public ChatMessageType Type { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long? TeamId { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long? EventId { get; set; }
}
