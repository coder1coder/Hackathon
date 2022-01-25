﻿using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Abstraction
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
        /// <param name="getFilterModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<UserModel>> GetAsync(GetFilterModel<UserFilterModel> getFilterModel);
        
        /// <summary>
        /// Проверка наличия пользователя по идентификатору
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns><c> true </c> если пользователь есть, иначе <c> false </c></returns>
        Task<bool> ExistAsync(long userId);
    }
}
