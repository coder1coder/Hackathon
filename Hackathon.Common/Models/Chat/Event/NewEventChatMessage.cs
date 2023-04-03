namespace Hackathon.Common.Models.Chat.Event;

public class NewEventChatMessage: BaseNewChatMessage, IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }

    public NewEventChatMessage() : base(ChatMessageType.EventChat)
    {
    }
}
