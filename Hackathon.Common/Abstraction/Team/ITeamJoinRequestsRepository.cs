using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Team;

public interface ITeamJoinRequestsRepository
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <returns></returns>
    Task CreateAsync(TeamJoinRequestCreateParameters createParameters);

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<TeamJoinRequestModel>> GetListAsync(GetListParameters<TeamJoinRequestExtendedFilter> parameters);

    /// <summary>
    /// Установить статус запроса
    /// </summary>
    /// <param name="joinRequestId">Идентификатор запроса</param>
    /// <param name="status">Статус</param>
    /// <returns></returns>
    Task SetStatusAsync(long joinRequestId, TeamJoinRequestStatus status);
}
