namespace Hackathon.Common.Models.Team;

public class TeamJoinRequestExtendedFilter: TeamJoinRequestFilter
{
    /// <summary>
    /// Идентификатор пользователя, автора запроса
    /// </summary>
    public long? UserId { get; set; }
}
