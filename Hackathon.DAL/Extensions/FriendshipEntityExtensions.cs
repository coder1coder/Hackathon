using Hackathon.Common.Models.Friend;
using Hackathon.Entities;

namespace Hackathon.DAL.Extensions;

public static class FriendshipEntityExtensions
{
    public static Friendship ToDto(this FriendshipEntity entity)
        => new()
        {
            ProposerId = entity.ProposerId,
            UserId = entity.UserId,
            Status = entity.Status
        };
}