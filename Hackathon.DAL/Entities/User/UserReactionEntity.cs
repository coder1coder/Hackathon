using Hackathon.Common.Models.User;

namespace Hackathon.DAL.Entities.User;

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
