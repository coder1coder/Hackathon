using System;

namespace Hackathon.Common.Models.Chat.Team;

public class CreateTeamChatMessage: ICreateChatMessage
{
    public long TeamId { get; set; }
    public long OwnerId { get; set; }
    public long? UserId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public ChatMessageType Type { get; set; }
    public ChatMessageOption Options { get; set; }
}