﻿using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Abstraction
{
    public interface IUserRepository
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="signUpModel"></param>
        /// <returns></returns>
        Task<UserModel> CreateAsync(SignUpModel signUpModel);

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
        /// Проверка наличия пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns><c> true </c> если пользователь есть, иначе <c> false </c></returns>
        Task<bool> ExistAsync(long userId);

        /// <summary>
        /// Получить информацию о пользователе по Google Id
        /// </summary>
        /// <param name="googleId"></param>
        /// <returns></returns>
        Task<UserModel?> GetByGoogleIdAsync(string googleId);

        /// <summary>
        /// Обновить информацию о Google аккаунте
        /// </summary>
        /// <param name="googleAccountModel"></param>
        /// <returns></returns>
        Task UpdateGoogleAccount(GoogleAccountModel googleAccountModel);
    }
}