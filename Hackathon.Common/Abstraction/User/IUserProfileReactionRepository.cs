using Hackathon.Common.Models.User;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

public interface IUserProfileReactionRepository
{
    /// <summary>
    /// Получить все реакции на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <returns></returns>
    Task<UserProfileReaction> GetReactionsAsync(long userId, long targetUserId);

    /// <summary>
    /// Добавить реакцию на профиль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
    /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
    /// <param name="reactions">Реакция</param>
    /// <returns></returns>
    Task UpsertReactionsAsync(long userId, long targetUserId, UserProfileReaction reactions);
}
