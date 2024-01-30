using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;

namespace Hackathon.API.Controllers.Team;

[SwaggerTag("Команды закрытого типа")]
[Route("api/team")]
public class PrivateTeamController(IPrivateTeamService privateTeamService, IMapper mapper) : BaseController
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpPost("{teamId:long}/join/request")]
    [ProducesResponseType(typeof(BaseCreateResponse), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> CreateJoinRequest([FromRoute] long teamId)
    {
        var createResult = await privateTeamService.CreateJoinRequestAsync(new TeamJoinRequestCreateParameters
        {
            TeamId = teamId,
            UserId = AuthorizedUserId
        });

        if (!createResult.IsSuccess)
            return await GetResult(() => Task.FromResult(createResult));

        return Ok(new BaseCreateResponse
        {
            Id = createResult.Data
        });
    }

    /// <summary>
    /// Принять пользователя в закрытую команду
    /// </summary>
    /// <returns></returns>
    [HttpPost("join/request/{requestId:long}/approve")]
    public Task<IActionResult> ApproveJoinRequest([FromRoute] long requestId)
        => GetResult(() => privateTeamService.ApproveJoinRequest(AuthorizedUserId, requestId));

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <returns></returns>
    [HttpPost("join/request/cancel")]
    public Task<IActionResult> CancelJoinRequest([FromBody] CancelRequestParameters parameters)
        => GetResult(() => privateTeamService.CancelJoinRequestAsync(AuthorizedUserId, parameters));

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpGet("{teamId:long}/join/request/sent")]
    [ProducesResponseType(typeof(TeamJoinRequestModel), (int)HttpStatusCode.OK)]
    public Task<IActionResult> GetSingleSentJoinRequest([FromRoute] long teamId)
        => GetResult(() => privateTeamService.GetSingleSentJoinRequestAsync(teamId, AuthorizedUserId));

    /// <summary>
    /// Получить список запросов пользователя на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpPost("join/request/list")]
    [ProducesResponseType(typeof(BaseCollection<TeamJoinRequestFilter>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetJoinRequests([FromBody] GetListParameters<TeamJoinRequestFilter> parameters)
    {
        var extendedFilter = mapper.Map<GetListParameters<TeamJoinRequestFilter>, GetListParameters<TeamJoinRequestExtendedFilter>>(parameters);
        extendedFilter.Filter.UserId = AuthorizedUserId;

        var result = await privateTeamService.GetJoinRequestsAsync(extendedFilter);
        return Ok(result);
    }

    /// <summary>
    /// Получить список отправленных запросов на вступление в команду
    /// </summary>
    /// <returns></returns>
    [HttpPost("{teamId:long}/join/request/list")]
    [ProducesResponseType(typeof(BaseCollection<TeamJoinRequestFilter>), (int) HttpStatusCode.OK)]
    public Task<IActionResult> GetTeamSentJoinRequests([FromRoute] long teamId, [FromBody] PaginationSort paginationSort)
        => GetResult(() => privateTeamService.GetTeamSentJoinRequests(teamId, AuthorizedUserId, paginationSort));

}
