using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.User
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="signUpModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(SignUpModel signUpModel);

        /// <summary>
        /// Получить информацию о пользователе по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<UserModel> GetAsync(long userId);

        /// <summary>
        /// Получить информацию о пользователях
        /// </summary>
        /// <param name="parameters">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollection<UserModel>> GetAsync(GetListParameters<UserFilter> parameters);

        /// <summary>
        /// Проверка наличия пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns><c> true </c> если пользователь есть, иначе <c> false </c></returns>
        Task<bool> IsExistAsync(long userId);

        /// <summary>
        /// Получить информацию о пользователе по GoogleId или Email
        /// </summary>
        /// <param name="googleId">GoogleId</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        Task<UserModel?> GetByGoogleIdOrEmailAsync(string googleId, string email);

        /// <summary>
        /// Обновить информацию о Google аккаунте
        /// </summary>
        /// <param name="googleAccountModel"></param>
        /// <returns></returns>
        Task UpdateGoogleAccount(GoogleAccountModel googleAccountModel);

        /// <summary>
        /// Обновить картинку пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="profileImageId">Идентификатор картинки в хранилище</param>
        /// <returns></returns>
        Task UpdateProfileImageAsync(long userId, Guid profileImageId);

        /// <summary>
        /// Получить все реакции на профиль пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
        /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
        /// <returns></returns>
        Task<UserProfileReaction[]> GetReactionsAsync(long userId, long targetUserId);

        /// <summary>
        /// Добавить реакцию на профиль пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
        /// <param name="targetUserId">Идентификатор пользователя получающего реакцию</param>
        /// <param name="reaction">Реакция</param>
        /// <returns></returns>
        Task AddReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);

        /// <summary>
        /// Удалить реакцию на профиль пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
        /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
        /// <param name="reaction">Реакция</param>
        /// <returns></returns>
        Task RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction);
    }
}
