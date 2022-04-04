using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.ProjectMember;

public class ProjectMemberModel
{
    public long UserId { get; set; }
    public UserModel User { get; set; }

    public long TeamId { get; set; }
    public TeamModel Team { get; set; }

    public long ProjectId { get; set; }
    public ProjectModel Project { get; set; }

    public long EventId { get; set; }
    public EventModel Event { get; set; }
}