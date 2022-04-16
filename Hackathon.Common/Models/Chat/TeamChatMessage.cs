using System;

namespace Hackathon.Common.Models.Chat;

public class TeamChatMessage: IChatMessage
{
    public long TeamId { get; set; }
    
    public long OwnerId { get; set; }
    public long? UserId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public ChatMessageContext Context { get; set; }
    public ChatMessageType Type { get; set; }
    public string OwnerFullName { get; set; }
    public string UserFullName { get; set; }
}