namespace Hackathon.Chats.Abstractions.Models.Events;

/// <summary>
/// Новое собщение чата мероприятия
/// </summary>
public class NewEventChatMessage: BaseNewChatMessage, IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
