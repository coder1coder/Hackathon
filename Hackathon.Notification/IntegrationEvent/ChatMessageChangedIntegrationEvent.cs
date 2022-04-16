using Hackathon.Common.Models.Chat;

namespace Hackathon.Notification.IntegrationEvent;

public class ChatMessageChangedIntegrationEvent: IIntegrationEvent
{
    public ChatMessageContext Context { get; set; } 
    public long? TeamId { get; set; }
}