namespace Hackathon.Common.Models.Teams;

public class TeamJoinRequestExtendedFilter: TeamJoinRequestFilter
{
    /// <summary>
    /// Идентификатор пользователя, автора запроса
    /// </summary>
    public long? UserId { get; set; }
}
