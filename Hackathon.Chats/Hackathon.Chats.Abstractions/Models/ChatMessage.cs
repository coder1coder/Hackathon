using System;

namespace Hackathon.Chats.Abstractions.Models;

public abstract class BaseChatMessage: BaseNewChatMessage, IChatMessage
{
    public Guid MessageId { get; set; }
    public long OwnerId { get; set; }
    public DateTime Timestamp { get; set; }

    public string OwnerFullName { get; set; }
    public string UserFullName { get; set; }
}
