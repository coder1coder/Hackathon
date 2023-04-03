namespace Hackathon.Common.Models.Chat.Team;

public class TeamChatMessage: BaseChatMessage, ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    public TeamChatMessage() : base(ChatMessageType.TeamChat)
    {
    }
}
