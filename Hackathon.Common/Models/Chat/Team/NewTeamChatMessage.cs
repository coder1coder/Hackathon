namespace Hackathon.Common.Models.Chat.Team;

public class NewTeamChatMessage: BaseNewChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    public NewTeamChatMessage():base(ChatMessageType.TeamChat)
    {
    }
}
