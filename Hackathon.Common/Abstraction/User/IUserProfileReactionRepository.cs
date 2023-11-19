using Hackathon.Common.Models.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

public interface IUserProfileReactionRepository
{
    /// <summary>
    /// Получить все реакции на профиль пользователя, поставленные одним пользователем 
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <returns></returns>
    Task<UserProfileReaction> GetReactionsAsync(long userId, long targetUserId);

    /// <summary>
    /// Получить все реакции на профиль пользователя, поставленные другими пользователями
    /// </summary>
    /// <param name="targetUserId">Идентификатор пользователя получившего реакции</param>
    /// <returns></returns>
    Task<List<UserProfileReaction>> GetReactionsAsync(long targetUserId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reactions">Реакция</param>
    /// <returns></returns>
    Task UpsertReactionsAsync(long userId, long targetUserId, UserProfileReaction reactions);
}
