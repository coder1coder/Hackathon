using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;

namespace Hackathon.Common.Abstraction.Team;

public interface ITeamJoinRequestsRepository
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    Task<long> CreateAsync(TeamJoinRequestCreateParameters createParameters);

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    Task<BaseCollection<TeamJoinRequestModel>> GetListAsync(GetListParameters<TeamJoinRequestExtendedFilter> parameters);

    /// <summary>
    /// Установить статус запроса с комментарием
    /// </summary>
    /// <param name="joinRequestId">Идентификатор запроса</param>
    /// <param name="status">Статус</param>
    /// <param name="comment">Комментарий</param>
    Task SetStatusWithCommentAsync(long joinRequestId, TeamJoinRequestStatus status, string comment = null);

    /// <summary>
    /// Получить запрос по идентификатору
    /// </summary>
    /// <param name="requestId">Идентификатор запроса</param>
    Task<TeamJoinRequestModel> GetAsync(long requestId);
}
