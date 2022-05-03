using Hackathon.Common.Models.Chat;

namespace Hackathon.Entities;

/// <summary>
/// Сообщение чата
/// </summary>
public class ChatMessageEntity
{
    /// <summary>
    /// Идентификуатор владельца сообщения
    /// </summary>
    public long OwnerId { get; set; }
    public string OwnerFullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long? UserId { get; set; }
    public string UserFullName { get; set; } = string.Empty;
    
    public long TeamId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public ChatMessageContext Context { get; set; }
    public ChatMessageType Type { get; set; }
}

public static class ChatMessageEntityExtensions
{
    public static TeamChatMessage ToTeamChatMessage(this ChatMessageEntity entity) => new()
        {
            Context = entity.Context,
            Message = entity.Message,
            Timestamp = entity.Timestamp,
            Type = entity.Type,
            OwnerId = entity.OwnerId,
            TeamId = entity.TeamId,
            OwnerFullName = entity.OwnerFullName,
            UserFullName = entity.UserFullName,
            UserId = entity.UserId
        };
}