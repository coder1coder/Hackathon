namespace Hackathon.Chats.Abstractions.Models.Teams;

public interface ITeamChatMessage
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
}
