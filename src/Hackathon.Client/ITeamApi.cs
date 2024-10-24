﻿using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Contracts.Teams;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Teams;
using Refit;

namespace Hackathon.Client;

public interface ITeamApi
{
    private const string BaseRoute = "/api/Team";

    [Post(BaseRoute)]
    Task<IApiResponse<BaseCreateResponse>> CreateAsync([Body] CreateTeamRequest createTeamRequest);

    [Get(BaseRoute + "/{id}")]
    Task<TeamModel> Get(long id);

    [Post(BaseRoute + "/getTeams")]
    Task<IApiResponse<BaseCollection<TeamModel>>> GetListAsync([Body] GetListParameters<TeamFilter> parameters);

    [Post(BaseRoute + "/{teamId}/join/request")]
    Task<BaseCreateResponse> CreateJoinRequestAsync(long teamId);

    /// <summary>
    /// Принять пользователя в закрытую команду
    /// </summary>
    [Post(BaseRoute + "/join/request/{requestId}/approve")]
    Task ApproveJoinRequest(long requestId);

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    [Post(BaseRoute + "/join/request/cancel")]
    Task CancelJoinRequestAsync([Body] CancelRequestParameters parameters);

    [Get(BaseRoute + "/{teamId}/join/request/sent")]
    Task<IApiResponse<TeamJoinRequestModel>> GetSentJoinRequestAsync(long teamId);

    /// <summary>
    /// Получить список запросов пользователя на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    [Post(BaseRoute + "/join/request/list")]
    Task<IApiResponse<BaseCollection<TeamJoinRequestModel>>> GetJoinRequests([Body] GetListParameters<TeamJoinRequestFilter> parameters);

    [Post(BaseRoute + "/{teamId}/join/request/list")]
    Task<IApiResponse<BaseCollection<TeamJoinRequestModel>>> GetTeamSentJoinRequestsAsync(long teamId, [Body] PaginationSort parameters);

    /// <summary>
    /// Вступить в открытую команду
    /// </summary>
    [Post(BaseRoute + "/{teamId}/join")]
    Task JoinToTeam(long teamId);
}
