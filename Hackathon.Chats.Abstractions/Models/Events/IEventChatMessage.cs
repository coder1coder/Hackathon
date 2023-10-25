namespace Hackathon.Chats.Abstractions.Models.Events;

public interface IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
