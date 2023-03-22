namespace Hackathon.Common.Models.Team;

public class TeamJoinRequestParameters: TeamJoinRequestCreateParameters
{
    /// <summary>
    /// Статус запроса
    /// </summary>
    public TeamJoinRequestStatus Status { get; set; }
}
