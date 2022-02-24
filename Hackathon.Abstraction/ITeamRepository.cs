using System.Linq.Expressions;
using Hackathon.Abstraction.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.Abstraction
{
    public interface ITeamRepository
    {
        /// <summary>
        /// Создание команды
        /// </summary>
        /// <param name="createTeamModel">Команда</param>
        /// <returns></returns>
        Task<long> CreateAsync(CreateTeamModel createTeamModel);

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

        /// <summary>
        /// Проверка наличия команды по имени
        /// </summary>
        /// <param name="teamName">Имя команды</param>
        /// <returns><c> true </c> если команда есть, иначе <c> false </c></returns>
        Task<bool> ExistAsync(string teamName);

        /// <summary>
        /// Проверка наличия команды по идентификатору
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns><c> true </c> если команда есть, иначе <c> false </c></returns>
        Task<bool> ExistAsync(long teamId);

        /// <summary>
        /// Добавление пользователя в команду
        /// </summary>
        /// <param name="teamMemberModel">Команда, пользователь</param>
        /// <returns></returns>
        Task AddMemberAsync(TeamMemberModel teamMemberModel);

        Task RemoveMemberAsync(TeamMemberModel teamMemberModel);

        Task<TeamModel[]> GetByExpression(Expression<Func<TeamEntity, bool>> expression);
    }
}