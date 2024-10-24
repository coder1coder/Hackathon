using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Abstraction.User;

public interface IUserService
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
    Task<Result<UserModel>> GetAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <param name="getListParameters">Фильтр, пагинация</param>
    Task<BaseCollection<UserModel>> GetListAsync(Models.GetListParameters<UserFilter> getListParameters);

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="file">Файл изображения</param>
    Task<Result<Guid>> UploadProfileImageAsync(long userId, IFormFile file);

    /// <summary>
    /// Редактирование профиля пользователя
    /// </summary>
    /// <param name="updateUserParameters">Данные для обновления профиля пользователя</param>
    Task<Result> UpdateUserAsync(UpdateUserParameters updateUserParameters);

    /// <summary>
    /// Обновить пароль пользователя
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры</param>
    Task<Result> UpdatePasswordAsync(long authorizedUserId, UpdatePasswordModel parameters);
}
