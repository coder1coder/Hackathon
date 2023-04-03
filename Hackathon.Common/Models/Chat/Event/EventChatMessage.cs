namespace Hackathon.Common.Models.Chat.Event;

public class EventChatMessage: BaseChatMessage, IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }

    public EventChatMessage() : base(ChatMessageType.EventChat)
    {
    }
}
