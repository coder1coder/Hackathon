using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;

namespace Hackathon.Common.Abstraction.Team;

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
    /// <param name="parameters">Фильтр, пагинация</param>
    /// <returns></returns>
    Task<BaseCollection<TeamModel>> GetListAsync(GetListParameters<TeamFilter> parameters);

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

    /// <summary>
    /// Удалить участника из команды
    /// </summary>
    /// <param name="teamMemberModel"></param>
    /// <returns></returns>
    Task RemoveMemberAsync(TeamMemberModel teamMemberModel);

    /// <summary>
    /// Изменить владельца команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="ownerId">Идентификатор нового владельца</param>
    /// <returns></returns>
    Task SetOwnerAsync(long teamId, long ownerId);

    /// <summary>
    /// Удалить команду и исключить из событий
    /// </summary>
    /// <param name="deleteTeamModel"></param>
    /// <returns></returns>
    Task DeleteTeamAsync(DeleteTeamModel deleteTeamModel);

    /// <summary>
    /// Получить количество участников команды + владелец команды (если есть)
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <returns>Общее количество участников команды</returns>
    Task<int> GetMembersCountAsync(long teamId);

    /// <summary>
    /// Получить идентификаторы членов команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="excludeMemberId">Идентификатор который нужно исключить</param>
    /// <returns></returns>
    Task<long[]> GetTeamMemberIdsAsync(long teamId, long? excludeMemberId = null);
}
