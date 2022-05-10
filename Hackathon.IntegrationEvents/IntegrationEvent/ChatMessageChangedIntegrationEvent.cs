using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Common.Models.Chat;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public class ChatMessageChangedIntegrationEvent: IIntegrationEvent
{
    public ChatMessageContext Context { get; set; } 
    public long? TeamId { get; set; }
}