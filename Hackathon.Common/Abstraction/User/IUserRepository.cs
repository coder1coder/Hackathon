using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using System;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

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
    Task<bool> ExistsAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователе по GoogleId или Email
    /// </summary>
    /// <param name="googleId">GoogleId</param>
    /// <param name="email">Email</param>
    /// <returns></returns>
    Task<UserModel> GetByGoogleIdOrEmailAsync(string googleId, string email);

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
    /// Обновление профиля пользователя
    /// </summary>
    /// <param name="updateUserParameters">Данные для обновления профиля пользователя</param>
    /// <returns></returns>
    Task UpdateAsync(UpdateUserParameters updateUserParameters);

    /// <summary>
    /// Обновить роль для записи пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="userRole">Роль</param>
    /// <returns></returns>
    Task SetRole(long userId, UserRole userRole);

    /// <summary>
    /// Получить роль пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<UserRole?> GetRoleAsync(long userId);

    /// <summary>
    /// Получить идентификаторы администраторов
    /// </summary>
    /// <returns></returns>
    Task<long[]> GetAdministratorIdsAsync();
}
