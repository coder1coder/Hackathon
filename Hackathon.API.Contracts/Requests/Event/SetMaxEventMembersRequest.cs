namespace Hackathon.Contracts.Requests.Event
{
    public class SetMaxEventMembersRequest
    {
        public long Id { get; set; }
        public int MaxEventMembers { get; set; }
    }
}