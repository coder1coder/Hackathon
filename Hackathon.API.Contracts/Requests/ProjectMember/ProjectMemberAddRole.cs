namespace Hackathon.Contracts.Requests.ProjectMember;

public class ProjectMemberAddRole
{
    public long ProjectMemberId { get; set; }
    public string ProjectMemberRole { get; set; }
}