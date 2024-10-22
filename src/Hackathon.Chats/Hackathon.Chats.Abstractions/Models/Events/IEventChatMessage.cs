namespace Hackathon.Chats.Abstractions.Models.Events;

/// <summary>
/// Сообщение чата мероприятия
/// </summary>
public interface IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
