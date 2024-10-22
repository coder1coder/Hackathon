namespace Hackathon.Common.Models.Teams;

public class TeamJoinRequestFilter
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long? TeamId { get; set; }

    /// <summary>
    /// Статус заявки
    /// </summary>
    public TeamJoinRequestStatus? Status { get; set; }
}
