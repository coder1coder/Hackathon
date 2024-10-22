namespace Hackathon.Common.Models.Teams;

public class TeamJoinRequestParameters: TeamJoinRequestCreateParameters
{
    /// <summary>
    /// Статус запроса
    /// </summary>
    public TeamJoinRequestStatus Status { get; set; }
}
