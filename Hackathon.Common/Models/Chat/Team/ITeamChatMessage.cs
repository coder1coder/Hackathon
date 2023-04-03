namespace Hackathon.Common.Models.Chat.Team;

public interface ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
