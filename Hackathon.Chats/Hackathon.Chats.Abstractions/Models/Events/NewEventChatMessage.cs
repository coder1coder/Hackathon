namespace Hackathon.Chats.Abstractions.Models.Events;

public class NewEventChatMessage: BaseNewChatMessage, IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
