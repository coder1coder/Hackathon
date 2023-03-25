using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Team;

public interface IPrivateTeamService
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <returns></returns>
    Task<Result<long>> CreateJoinRequestAsync(TeamJoinRequestCreateParameters parameters);

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры отмены</param>
    /// <returns></returns>
    Task<Result> CancelJoinRequestAsync(long authorizedUserId, CancelRequestParameters parameters);

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result<TeamJoinRequestModel>> GetSingleSentJoinRequestAsync(long teamId, long userId);

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<TeamJoinRequestModel>> GetJoinRequestsAsync(Models.GetListParameters<TeamJoinRequestExtendedFilter> parameters);

    /// <summary>
    /// Получить список отправленных запросов на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="authorizedUserId">Идентификатор пользователя осуществляющего запрос</param>
    /// <param name="paginationSort">Параметры пагинации и сортировки</param>
    /// <returns></returns>
    Task<Result<BaseCollection<TeamJoinRequestModel>>> GetTeamSentJoinRequests(long teamId, long authorizedUserId, PaginationSort paginationSort);
}
