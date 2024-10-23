using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;

namespace Hackathon.Common.Abstraction.Team;

public interface ITeamService
{
    /// <summary>
    /// Создание команды
    /// </summary>
    /// <param name="createTeamModel">Команда</param>
    /// <param name="userId">Идентификатор авторизованного пользователя</param>
    Task<Result<long>> CreateAsync(CreateTeamModel createTeamModel, long? userId = null);

    /// <summary>
    /// Добавление пользователя в команду
    /// </summary>
    /// <param name="teamMemberModel">Команда, пользователь</param>
    Task<Result> AddMemberAsync(TeamMemberModel teamMemberModel);

    /// <summary>
    /// Получение информации о команде по идентификатору
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    Task<Result<TeamModel>> GetAsync(long teamId);

    /// <summary>
    /// Получение информации о командах
    /// </summary>
    /// <param name="getListParameters">Фильтр, пагинация</param>
    Task<BaseCollection<TeamModel>> GetListAsync(Common.Models.GetListParameters<TeamFilter> getListParameters);

    /// <summary>
    /// Получить команду пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result<TeamGeneral>> GetUserTeam(long userId);

    /// <summary>
    /// Удалить участника из команды
    /// </summary>
    /// <param name="teamMemberModel"></param>
    Task<Result> RemoveMemberAsync(TeamMemberModel teamMemberModel);

    /// <summary>
    /// Получить список событий в которых принимала участие команда
    /// список включает в себя данные о проекте
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="paginationSort">Пагинация и сортировка</param>
    Task<Result<BaseCollection<TeamEventListItem>>> GetTeamEvents(long teamId, PaginationSort paginationSort);
}
