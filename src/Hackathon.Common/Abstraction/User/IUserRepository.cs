using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using System;
using System.Threading.Tasks;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.User;

public interface IUserRepository
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="createNewUserModel"></param>
    Task<long> CreateAsync(CreateNewUserModel createNewUserModel);

    /// <summary>
    /// Получить информацию о пользователе по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<UserModel> GetAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <param name="parameters">Фильтр, пагинация</param>
    Task<BaseCollection<UserModel>> GetAsync(GetListParameters<UserFilter> parameters);

    /// <summary>
    /// Получить данные пользователя для авторизации в системе
    /// </summary>
    /// <param name="userName">Логин пользователя</param>
    Task<UserSignInDetails> GetUserSignInDetailsAsync(string userName);

    /// <summary>
    /// Проверка наличия пользователя по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns><c> true </c> если пользователь есть, иначе <c> false </c></returns>
    Task<bool> ExistsAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователе по GoogleId или Email
    /// </summary>
    /// <param name="googleId">GoogleId</param>
    /// <param name="email">Email</param>
    Task<UserModel> GetByGoogleIdOrEmailAsync(string googleId, string email);

    /// <summary>
    /// Обновить информацию о Google аккаунте
    /// </summary>
    /// <param name="googleAccountModel"></param>
    Task UpdateGoogleAccount(GoogleAccountModel googleAccountModel);

    /// <summary>
    /// Обновить картинку пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="profileImageId">Идентификатор картинки в хранилище</param>
    Task UpdateProfileImageAsync(long userId, Guid profileImageId);

    /// <summary>
    /// Обновление профиля пользователя
    /// </summary>
    /// <param name="updateUserParameters">Данные для обновления профиля пользователя</param>
    Task UpdateAsync(UpdateUserParameters updateUserParameters);

    /// <summary>
    /// Обновить роль для записи пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userRole">Роль</param>
    Task SetRole(long userId, UserRole userRole);

    /// <summary>
    /// Получить роль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<UserRole?> GetRoleAsync(long userId);

    /// <summary>
    /// Получить идентификаторы администраторов
    /// </summary>
    Task<long[]> GetAdministratorIdsAsync();

    /// <summary>
    /// Получить хеш пароля пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<string> GetPasswordHashAsync(long userId);

    /// <summary>
    /// Обновить хешированный пароль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="passwordHash">Хешированный пароль</param>
    Task UpdatePasswordHashAsync(long userId, string passwordHash);
}
