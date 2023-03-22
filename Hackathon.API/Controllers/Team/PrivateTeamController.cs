using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers.Team;

[SwaggerTag("Команды закрытого типа")]
[Route("api/team")]
public class PrivateTeamController: BaseController
{
    private readonly IPrivateTeamService _privateTeamService;

    public PrivateTeamController(IPrivateTeamService privateTeamService)
    {
        _privateTeamService = privateTeamService;
    }

    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpPost("{teamId:long}/join/request")]
    public Task<IActionResult> CreateJoinRequest([FromRoute] long teamId)
        => GetResult(() => _privateTeamService.CreateJoinRequestAsync(new TeamJoinRequestCreateParameters
        {
            TeamId = teamId,
            UserId = AuthorizedUserId,
        }));

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpDelete("{teamId:long}/join/request")]
    public Task<IActionResult> CancelJoinRequest([FromRoute] long teamId)
        => GetResult(() => _privateTeamService.CancelJoinRequestAsync(teamId, AuthorizedUserId));

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId"></param>
    /// <returns></returns>
    [HttpGet("{teamId:long}/join/request/sent")]
    [ProducesResponseType(typeof(TeamJoinRequestModel), (int)HttpStatusCode.OK)]
    public Task<IActionResult> GetSentJoinRequest([FromRoute] long teamId)
        => GetResult(() => _privateTeamService.GetSentJoinRequestAsync(teamId, AuthorizedUserId));

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    [HttpPost("join/request/list")]
    [ProducesResponseType(typeof(BaseCollection<TeamJoinRequestFilter>), (int) HttpStatusCode.OK)]
    public async Task<IActionResult> GetJoinRequests([FromBody] GetListParameters<TeamJoinRequestFilter> parameters)
        => Ok(await _privateTeamService.GetJoinRequestsAsync(AuthorizedUserId, parameters));
}
