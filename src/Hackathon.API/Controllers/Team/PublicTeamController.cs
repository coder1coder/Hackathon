using Hackathon.Common.Abstraction.Team;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using Hackathon.API.Module;

namespace Hackathon.API.Controllers.Team;

[SwaggerTag("Команды открытого типа")]
[Route("api/team")]
public class PublicTeamController : BaseController
{
    private readonly IPublicTeamService _publicTeamService;

    public PublicTeamController(IPublicTeamService publicTeamService)
    {
        _publicTeamService = publicTeamService;
    }

    /// <summary>
    /// Вступить в открытую команду
    /// </summary>
    [HttpPost("{teamId:long}/join")]
    public Task<IActionResult> JoinToTeam([FromRoute] long teamId)
        => GetResult(() => _publicTeamService.JoinToTeamAsync(teamId, AuthorizedUserId));
}
