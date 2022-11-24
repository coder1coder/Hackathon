using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.User;

public interface IUserProfileReactionService
{
    /// <summary>
    /// Добавить/обновить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    /// <returns></returns>
    Task UpsertReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);

    /// <summary>
    /// Удалить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    /// <returns></returns>
    Task RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);

    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
    /// <returns></returns>
    Task<UserProfileReaction?> GetReactionsAsync(long userId, long targetUserId);
}
