namespace Hackathon.Contracts.Requests.Event
{
    public class SetMinTeamMembersRequest
    {
        public long Id { get; set; }
        public int MinTeamMembers { get; set; }
    }
}