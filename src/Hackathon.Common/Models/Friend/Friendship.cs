using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Models.Friend;

public class Friendship
{
    /// <summary>
    /// Инициатор предложения дружбы
    /// </summary>
    public long ProposerId { get; set; }
    public UserModel Proposer { get; set; }
    
    /// <summary>
    /// Кому адресовано предложение дружбы
    /// </summary>
    public long UserId { get; set; }
    public UserModel User { get; set; }
    public FriendshipStatus Status { get; set; }
}
