using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction.User
{
    public interface IUserService
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="signUpModel"></param>
        /// <returns></returns>
        Task<long> CreateAsync(SignUpModel signUpModel);

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="signInModel"></param>
        /// <returns></returns>
        Task<AuthTokenModel> SignInAsync(SignInModel signInModel);

        /// <summary>
        /// Авторизация пользователя через Google
        /// </summary>
        /// <param name="signInByGoogleModel"></param>
        /// <returns></returns>
        Task<AuthTokenModel> SignInByGoogle(SignInByGoogleModel signInByGoogleModel);

        /// <summary>
        /// Получить информацию о пользователе по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<UserModel> GetAsync(long userId);

        /// <summary>
        /// Получить информацию о пользователях
        /// </summary>
        /// <param name="getListParameters">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollection<UserModel>> GetAsync(GetListParameters<UserFilter> getListParameters);

        /// <summary>
        /// Сгенерировать токен
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        AuthTokenModel GenerateToken(UserModel user);

        /// <summary>
        /// Загрузить картинку профиля пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="filename">Имя файла картинки</param>
        /// <param name="stream">Новая картинка профиля</param>
        /// <returns></returns>
        Task<Guid> UploadProfileImageAsync(long userId, string filename, Stream stream);

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

        /// <summary>
        /// Получить все реакции на профиль пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя инициировавшего реакцию</param>
        /// <param name="targetUserId">Идентификатор пользователя получившего реакцию</param>
        /// <returns></returns>
        Task<UserProfileReaction[]> GetReactionsAsync(long userId, long targetUserId);
    }
}
