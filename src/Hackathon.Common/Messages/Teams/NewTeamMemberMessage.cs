namespace Hackathon.Common.Messages.Teams;

public record NewTeamMemberMessage
{
    public long TeamId { get; }
    public long MemberId { get; }

    public NewTeamMemberMessage(long teamId, long memberId)
    {
        TeamId = teamId;
        MemberId = memberId;
    }
}
