using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.Abstraction
{
    public interface ITeamService
    {
        /// <summary>
        /// Создание команды
        /// </summary>
        /// <param name="createTeamModel">Команда</param>
        /// <returns></returns>
        Task<long> CreateAsync(CreateTeamModel createTeamModel);

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
        Task<TeamModel> GetAsync(long teamId);

        /// <summary>
        /// Получение информации о командах
        /// </summary>
        /// <param name="getListModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<TeamModel>> GetAsync(GetListModel<TeamFilterModel> getListModel);

        Task<TeamModel> GetUserTeam(long userId);
    }
}