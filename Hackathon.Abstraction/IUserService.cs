﻿using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction
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
        /// Регистрация пользователя
        /// </summary>
        /// <param name="signInModel"></param>
        /// <returns></returns>
        Task<AuthTokenModel> SignInAsync(SignInModel signInModel);

        /// <summary>
        /// Получить информацию о пользователе по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<UserModel> GetAsync(long userId);

        /// <summary>
        /// Получить информацию о пользователях
        /// </summary>
        /// <param name="getListModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<UserModel>> GetAsync(GetListModel<UserFilterModel> getListModel);

        /// <summary>
        /// Сгенерировать токен
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        AuthTokenModel GenerateToken(UserModel user);
    }
}