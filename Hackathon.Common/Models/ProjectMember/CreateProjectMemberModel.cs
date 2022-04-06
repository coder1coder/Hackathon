using System.Collections.Generic;

namespace Hackathon.Common.Models.ProjectMember;

public class CreateProjectMemberModel
{
    public long UserId { get; set; }
    
    public long TeamId { get; set; }

    public long ProjectId { get; set; }

    public long? EventId { get; set; }
    
    public List<string>? ProjectMemberRoles { get; set; }
}