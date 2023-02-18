﻿using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.Abstraction.Team
{
    public interface ITeamService
    {
        /// <summary>
        /// Создание команды
        /// </summary>
        /// <param name="createTeamModel">Команда</param>
        /// <param name="userId">Идентификатор авторизованного пользователя</param>
        /// <returns></returns>
        Task<long> CreateAsync(CreateTeamModel createTeamModel, long? userId = null);

        /// <summary>
        /// Добавление пользователя в команду
        /// </summary>
        /// <param name="teamMemberModel">Команда, пользователь</param>
        /// <returns></returns>
        Task AddMemberAsync(TeamMemberModel teamMemberModel);

        /// <summary>
        /// Получение информации о команде по идентификатору
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns></returns>
        Task<Result<TeamModel>> GetAsync(long teamId);

        /// <summary>
        /// Получение информации о командах
        /// </summary>
        /// <param name="getListParameters">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollection<TeamModel>> GetAsync(Common.Models.GetListParameters<TeamFilter> getListParameters);

        /// <summary>
        /// Получить команду пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<Result<TeamGeneral>> GetUserTeam(long userId);

        /// <summary>
        /// Удалить участника из команды
        /// </summary>
        /// <param name="teamMemberModel"></param>
        /// <returns></returns>
        Task RemoveMemberAsync(TeamMemberModel teamMemberModel);

        /// <summary>
        /// Получить список событий в которых принимала участие команда
        /// список включает в себя данные о проекте
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <param name="paginationSort">Пагинация и сортировка</param>
        /// <returns></returns>
        Task<Result<BaseCollection<TeamEventListItem>>> GetTeamEvents(long teamId, PaginationSort paginationSort);
    }
}
