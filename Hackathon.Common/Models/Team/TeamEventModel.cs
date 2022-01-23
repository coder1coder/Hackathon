using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;

namespace Hackathon.Common.Models.Team;

public class TeamEventModel
{
    public long TeamId { get; set; }
    public TeamModel Team { get; set; }

    public long EventId { get; set; }
    public EventModel Event { get; set; }

    public long ProjectId { get; set; }
    public ProjectModel Project { get; set; }
}