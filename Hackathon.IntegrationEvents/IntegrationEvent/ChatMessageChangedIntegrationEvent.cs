using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.Common.Models.Chat;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public sealed class ChatMessageChangedIntegrationEvent: IIntegrationEvent
{
    public ChatMessageType Type { get; set; }
    public long? TeamId { get; set; }
}
