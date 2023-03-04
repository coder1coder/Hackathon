using Hackathon.Common.Models.Friend;
using System;

namespace Hackathon.DAL.Entities;

public class FriendshipEntity
{
    public Guid Id { get; set; }
    public long ProposerId { get; set; }
    public long UserId { get; set; }

    public FriendshipStatus Status { get; set; }
}
