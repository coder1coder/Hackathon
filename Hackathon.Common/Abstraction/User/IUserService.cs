using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

public interface IUserService
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    /// <param name="createNewUserModel"></param>
    /// <returns></returns>
    Task<long> CreateAsync(CreateNewUserModel createNewUserModel);

    /// <summary>
    /// Получить информацию о пользователе по идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result<UserModel>> GetAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <param name="getListParameters">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<BaseCollection<UserModel>> GetListAsync(Models.GetListParameters<UserFilter> getListParameters);

    /// <summary>
    /// Загрузить картинку профиля пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <param name="file">Файл изображения</param>
    /// <returns></returns>
    Task<Result<Guid>> UploadProfileImageAsync(long userId, IFormFile file);

    /// <summary>
    /// Редактирование профиля пользователя
    /// </summary>
    /// <param name="updateUserParameters">Данные для обновления профиля пользователя</param>
    /// <returns></returns>
    Task<Result> UpdateUserAsync(UpdateUserParameters updateUserParameters);

    /// <summary>
    /// Обновить пароль пользователя
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры</param>
    /// <returns></returns>
    Task<Result> UpdatePasswordAsync(long authorizedUserId, UpdatePasswordModel parameters);
}
