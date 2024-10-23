using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using System.Threading.Tasks;
using Hackathon.Common.Models.Teams;

namespace Hackathon.Common.Abstraction.Team;

public interface IPrivateTeamService
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    Task<Result<long>> CreateJoinRequestAsync(TeamJoinRequestCreateParameters parameters);

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="parameters">Параметры отмены</param>
    Task<Result> CancelJoinRequestAsync(long authorizedUserId, CancelRequestParameters parameters);

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="userId">Идентификатор пользователя</param>
    Task<Result<TeamJoinRequestModel>> GetSingleSentJoinRequestAsync(long teamId, long userId);

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    Task<BaseCollection<TeamJoinRequestModel>> GetJoinRequestsAsync(Models.GetListParameters<TeamJoinRequestExtendedFilter> parameters);

    /// <summary>
    /// Получить список отправленных запросов на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="authorizedUserId">Идентификатор пользователя осуществляющего запрос</param>
    /// <param name="paginationSort">Параметры пагинации и сортировки</param>
    Task<Result<BaseCollection<TeamJoinRequestModel>>> GetTeamSentJoinRequests(long teamId, long authorizedUserId, PaginationSort paginationSort);

    /// <summary>
    /// Принять пользователя в закрытую команду
    /// </summary>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <param name="requestId">Идентификатор запроса на вступление</param>
    Task<Result> ApproveJoinRequest(long authorizedUserId, long requestId);
}
