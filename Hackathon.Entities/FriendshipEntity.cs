using Hackathon.Common.Models.Friend;

namespace Hackathon.Entities;

public class FriendshipEntity
{
    public Guid Id { get; set; }
    public long ProposerId { get; set; }
    public long UserId { get; set; }

    public FriendshipStatus Status { get; set; }
}
