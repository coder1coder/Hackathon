namespace Hackathon.Contracts.Requests.Event
{
    public class SetMaxTeamMembersRequest
    {
        public long Id { get; set; }
        public int MaxTeamMembers { get; set; }
    }
}