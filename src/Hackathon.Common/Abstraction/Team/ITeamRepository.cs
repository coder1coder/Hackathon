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
    Task<long> CreateAsync(CreateTeamModel createTeamModel);

    /// <summary>
    /// Получение информации о команде по идентификатору
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    Task<TeamModel> GetAsync(long teamId);

    /// <summary>
    /// Получение информации о командах
    /// </summary>
    /// <param name="parameters">Фильтр, пагинация</param>
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
    Task AddMemberAsync(TeamMemberModel teamMemberModel);

    /// <summary>
    /// Удалить участника из команды
    /// </summary>
    /// <param name="teamMemberModel"></param>
    Task RemoveMemberAsync(TeamMemberModel teamMemberModel);

    /// <summary>
    /// Изменить владельца команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="ownerId">Идентификатор нового владельца</param>
    Task SetOwnerAsync(long teamId, long ownerId);

    /// <summary>
    /// Удалить команду и исключить из событий
    /// </summary>
    /// <param name="deleteTeamModel"></param>
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
    Task<long[]> GetTeamMemberIdsAsync(long teamId, long? excludeMemberId = null);
}
