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

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long? TeamId { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    public string Message { get; set; }

    public DateTime Timestamp { get; set; }
    public ChatMessageType Type { get; set; }
    public ChatMessageOption Options { get; set; }
}

public static class ChatMessageEntityExtensions
{
    public static TeamChatMessage ToTeamChatMessage(this ChatMessageEntity entity) => new()
        {
            Type = entity.Type,
            Message = entity.Message,
            Timestamp = entity.Timestamp,
            Options = entity.Options,
            OwnerId = entity.OwnerId,
            TeamId = entity.TeamId,
            OwnerFullName = entity.OwnerFullName,
            UserFullName = entity.UserFullName,
            UserId = entity.UserId
        };
}
