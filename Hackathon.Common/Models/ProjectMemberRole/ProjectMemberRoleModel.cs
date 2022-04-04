using System.Collections.Generic;
using Hackathon.Common.Models.ProjectMember;

namespace Hackathon.Common.Models.ProjectMemberRole;

public class ProjectMemberRoleModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public List<ProjectMemberModel> Members { get; set; } = new();
    
}