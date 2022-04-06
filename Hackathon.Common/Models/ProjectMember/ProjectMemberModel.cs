using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.ProjectMember;

public class ProjectMemberModel
{
    public long UserId { get; set; }
    
    public long TeamId { get; set; }
    
    public long ProjectId { get; set; }
    
    public List<string> ProjectMemberRoles { get; set; }
}