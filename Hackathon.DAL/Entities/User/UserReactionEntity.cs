using Hackathon.Common.Models.User;

namespace Hackathon.DAL.Entities.User;

public class UserReactionEntity
{
    public long UserId { get; set; }
    public long TargetUserId { get; set; }
    public UserProfileReaction Reaction { get; set; }
}
