using Hackathon.Common.Abstraction.Team;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Hackathon.API.Module;

namespace Hackathon.API.Controllers.Team;

[SwaggerTag("Команды открытого типа")]
[Route("api/team")]
public class PublicTeamController(IPublicTeamService publicTeamService) : BaseController
{
    /// <summary>
    /// Вступить в открытую команду
    /// </summary>
    /// <returns></returns>
    [HttpPost("{teamId:long}/join")]
    public Task<IActionResult> JoinToTeam([FromRoute] long teamId)
        => GetResult(() => publicTeamService.JoinToTeamAsync(teamId, AuthorizedUserId));
}
