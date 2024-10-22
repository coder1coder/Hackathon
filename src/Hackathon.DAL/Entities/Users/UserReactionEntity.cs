using Hackathon.Common.Models.Users;

namespace Hackathon.DAL.Entities.Users;

/// <summary>
/// Реакция на профиль пользователя
/// </summary>
public class UserReactionEntity
{
    /// <summary>
    /// Идентификатор автора реакции
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор целевого пользователя
    /// </summary>
    public long TargetUserId { get; set; }

    /// <summary>
    /// Реакция
    /// </summary>
    public UserProfileReaction Reaction { get; set; }
}
