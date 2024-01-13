namespace Hackathon.Chats.Abstractions.Models.Events;

public class EventChatMessage: BaseChatMessage, IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
