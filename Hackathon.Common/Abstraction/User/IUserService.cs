using BackendTools.Common.Models;
using Hackathon.Common.Models;
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
    /// <param name="signUpModel"></param>
    /// <returns></returns>
    Task<long> CreateAsync(SignUpModel signUpModel);

    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    /// <param name="signInModel"></param>
    /// <returns></returns>
    Task<Result<AuthTokenModel>> SignInAsync(SignInModel signInModel);

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
    Task<Result<UserModel>> GetAsync(long userId);

    /// <summary>
    /// Получить информацию о пользователях
    /// </summary>
    /// <param name="getListParameters">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<BaseCollection<UserModel>> GetAsync(Models.GetListParameters<UserFilter> getListParameters);

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
}
