namespace Hackathon.Common.Models.Friend;

public enum FriendshipStatus
{
    /// <summary>
    /// Предлоджение дружбы отправлено
    /// </summary>
    Pending = 1,
    
    /// <summary>
    /// Предложение дружбы принято
    /// </summary>
    Confirmed = 2,
    
    /// <summary>
    /// Предложение дружбы отклонено
    /// </summary>
    Rejected = 3
}