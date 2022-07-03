using Hackathon.Common.Models;
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
        /// <param name="getListParameters">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollection<TeamModel>> GetAsync(GetListParameters<TeamFilter> getListParameters);

        /// <summary>
        /// Получить команду пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        Task<TeamGeneral> GetUserTeam(long userId);

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
        Task<BaseCollection<TeamEventListItem>> GetTeamEvents(long teamId, PaginationSort paginationSort);
    }
}
