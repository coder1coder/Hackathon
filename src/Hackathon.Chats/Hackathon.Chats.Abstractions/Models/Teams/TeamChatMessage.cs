namespace Hackathon.Chats.Abstractions.Models.Teams;

/// <summary>
/// Сообщение чата команды
/// </summary>
public class TeamChatMessage: BaseChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
