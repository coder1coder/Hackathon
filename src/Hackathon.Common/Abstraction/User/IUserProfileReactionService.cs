using BackendTools.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.User;

public interface IUserProfileReactionService
{
    /// <summary>
    /// Добавить/обновить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reaction">Реакция</param>
    /// <returns></returns>
    Task<Result> UpsertReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);

    /// <summary>
    /// Удалить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
    /// <param name="reaction">Реакция</param>
    /// <returns></returns>
    Task<Result> RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);

    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
    /// <returns></returns>
    Task<Result<UserProfileReaction?>> GetReactionsAsync(long userId, long targetUserId);

    /// <summary>
    /// Получить реакции на профиль пользователя по типу с количеством
    /// </summary>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакции</param>
    /// <returns></returns>
    Task<Result<List<UserProfileReactionModel>>> GetReactionsByTypeAsync(long targetUserId);
}
