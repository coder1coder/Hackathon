using System;

namespace Hackathon.Common.Models.Chat;

public interface ICreateChatMessage
{
    long OwnerId { get; set; }
    long? UserId { get; set; }
    string Message { get; set; }
    DateTime Timestamp { get; set; }
    ChatMessageContext Context { get; set; }
    ChatMessageType Type { get; set; }
}

public interface IChatMessage: ICreateChatMessage
{
    string OwnerFullName { get; set; }
    string UserFullName { get; set; }
}

public enum ChatMessageContext
{
    TeamChat = 0
}

public enum ChatMessageType
{
    Default = 0,
}