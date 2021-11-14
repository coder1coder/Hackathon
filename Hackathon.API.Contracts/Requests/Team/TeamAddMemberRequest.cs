namespace Hackathon.Contracts.Requests.Team
{
    public class TeamAddMemberRequest
    {
        public long TeamId { get; set; }
        public long UserId { get; set; }
    }
}