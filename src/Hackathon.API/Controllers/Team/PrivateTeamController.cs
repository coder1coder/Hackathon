using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Module;
using Hackathon.Common.Models.Teams;

namespace Hackathon.API.Controllers.Team;

[SwaggerTag("Команды закрытого типа")]
[Route("api/team")]
public class PrivateTeamController : BaseController
{
    private readonly IPrivateTeamService _privateTeamService;
    private readonly IMapper _mapper;

    public PrivateTeamController(IPrivateTeamService privateTeamService, IMapper mapper)
    {
        _privateTeamService = privateTeamService;
        _mapper = mapper;
    }

    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    [HttpPost("{teamId:long}/join/request")]
    [ProducesResponseType(typeof(BaseCreateResponse), (int) HttpStatusCode.OK)]
    public Task<IActionResult> CreateJoinRequest([FromRoute] long teamId)
    {
        return GetResult(() => _privateTeamService.CreateJoinRequestAsync(new TeamJoinRequestCreateParameters
            {
                TeamId = teamId,
                UserId = AuthorizedUserId
            }),
            result => new BaseCreateResponse
            {
                Id = result
            });
    }

    /// <summary>
    /// Принять пользователя в закрытую команду
    /// </summary>
    [HttpPost("join/request/{requestId:long}/approve")]
    public Task<IActionResult> ApproveJoinRequest([FromRoute] long requestId)
        => GetResult(() => _privateTeamService.ApproveJoinRequest(AuthorizedUserId, requestId));

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    [HttpPost("join/request/cancel")]
    public Task<IActionResult> CancelJoinRequest([FromBody] CancelRequestParameters parameters)
        => GetResult(() => _privateTeamService.CancelJoinRequestAsync(AuthorizedUserId, parameters));

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    [HttpGet("{teamId:long}/join/request/sent")]
    [ProducesResponseType(typeof(TeamJoinRequestModel), (int)HttpStatusCode.OK)]
    public Task<IActionResult> GetSingleSentJoinRequest([FromRoute] long teamId)
        => GetResult(() => _privateTeamService.GetSingleSentJoinRequestAsync(teamId, AuthorizedUserId));

    /// <summary>
    /// Получить список запросов пользователя на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    [HttpPost("join/request/list")]
    [ProducesResponseType(typeof(BaseCollection<TeamJoinRequestFilter>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetJoinRequests([FromBody] GetListParameters<TeamJoinRequestFilter> parameters)
    {
        var extendedFilter = _mapper.Map<GetListParameters<TeamJoinRequestFilter>, GetListParameters<TeamJoinRequestExtendedFilter>>(parameters);
        extendedFilter.Filter.UserId = AuthorizedUserId;

        var result = await _privateTeamService.GetJoinRequestsAsync(extendedFilter);
        return Ok(result);
    }

    /// <summary>
    /// Получить список отправленных запросов на вступление в команду
    /// </summary>
    [HttpPost("{teamId:long}/join/request/list")]
    [ProducesResponseType(typeof(BaseCollection<TeamJoinRequestFilter>), (int) HttpStatusCode.OK)]
    public Task<IActionResult> GetTeamSentJoinRequests([FromRoute] long teamId, [FromBody] PaginationSort paginationSort)
        => GetResult(() => _privateTeamService.GetTeamSentJoinRequests(teamId, AuthorizedUserId, paginationSort));

}
