namespace Hackathon.Common.Messages.Teams;

public record TeamMemberRemovedMessage(long TeamId, long MemberId)
{
    public long TeamId { get; } = TeamId;
    public long MemberId { get; } = MemberId;
}
