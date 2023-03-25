using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.Client;

public interface ITeamApi
{
    private const string BaseRoute = "/api/Team";

    [Post(BaseRoute)]
    Task<IApiResponse<BaseCreateResponse>> Create([Body] CreateTeamRequest createTeamRequest);

    [Get(BaseRoute + "/{id}")]
    Task<TeamModel> Get(long id);

    [Post(BaseRoute + "/getTeams")]
    Task<IApiResponse<BaseCollection<TeamModel>>> GetListAsync([Body] GetListParameters<TeamFilter> parameters);

    [Post(BaseRoute + "/{teamId}/join/request")]
    Task<BaseCreateResponse> CreateJoinRequestAsync(long teamId);

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <returns></returns>
    [Post(BaseRoute + "/join/request/cancel")]
    Task CancelJoinRequestAsync([Body] CancelRequestParameters parameters);

    [Get(BaseRoute + "/{teamId}/join/request/sent")]
    Task<IApiResponse<TeamJoinRequestModel>> GetSentJoinRequestAsync(long teamId);

    [Post(BaseRoute + "/join/request/list")]
    Task<IApiResponse<BaseCollection<TeamJoinRequestModel>>> GetSingleJoinRequestsAsync([Body] GetListParameters<TeamJoinRequestFilter> parameters);

    [Post(BaseRoute + "/{teamId}/join/request/list")]
    Task<IApiResponse<BaseCollection<TeamJoinRequestModel>>> GetTeamSentJoinRequestsAsync(long teamId, [Body] PaginationSort parameters);
}
