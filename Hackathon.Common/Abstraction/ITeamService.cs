using System.Threading.Tasks;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Abstraction
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
        /// <param name="teamAddMemberModel">Команда, пользователь</param>
        /// <returns></returns>
        Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel);
        
        /// <summary>
        /// Получение информации о команде по идентификатору
        /// </summary>
        /// <param name="teamId">Идентификатор команды</param>
        /// <returns></returns>
        Task<TeamModel> GetAsync(long teamId);
        
        /// <summary>
        /// Получение информации о командах
        /// </summary>
        /// <param name="getFilterModel">Фильтр, пагинация</param>
        /// <returns></returns>
        Task<BaseCollectionModel<TeamModel>> GetAsync(GetFilterModel<TeamFilterModel> getFilterModel);
    }
}