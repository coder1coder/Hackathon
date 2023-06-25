namespace Hackathon.Common.Models.Team;

public class TeamMemberModel
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public long MemberId { get; set; }
}
