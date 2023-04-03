using System;

namespace Hackathon.Common.Models.Chat;

public abstract class BaseChatMessage: BaseNewChatMessage, IChatMessage
{
    public long OwnerId { get; set; }
    public DateTime Timestamp { get; set; }

    public string OwnerFullName { get; set; }
    public string UserFullName { get; set; }

    protected BaseChatMessage(ChatMessageType chatMessageType):base(chatMessageType)
    {
    }
}
