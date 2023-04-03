namespace Hackathon.Common.Models.Chat.Event;

public interface IEventChatMessage
{
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
}
