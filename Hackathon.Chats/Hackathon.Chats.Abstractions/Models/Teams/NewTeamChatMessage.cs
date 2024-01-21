namespace Hackathon.Chats.Abstractions.Models.Teams;

/// <summary>
/// Новое сообщение чата команды
/// </summary>
public class NewTeamChatMessage: BaseNewChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
