namespace Hackathon.Chats.Abstractions.Models.Teams;

public class NewTeamChatMessage: BaseNewChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
