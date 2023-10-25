namespace Hackathon.Chats.Abstractions.Models.Teams;

public class TeamChatMessage: BaseChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
