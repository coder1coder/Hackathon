namespace Hackathon.Common.Models.Friend;

/// <summary>
/// Статус дружбы
/// </summary>
public enum FriendshipStatus: byte
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
