namespace Hackathon.Common.Models.Team;

public class TeamJoinRequestCreateParameters
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }
}
