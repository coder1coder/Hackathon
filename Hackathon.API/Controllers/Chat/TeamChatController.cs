using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Team;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers.Chat;

[Route("api/chat/team")]
public class TeamChatController: BaseController
{
    private readonly ITeamChatService _teamChatService;

    public TeamChatController(ITeamChatService teamChatService)
    {
        _teamChatService = teamChatService;
    }

    /// <summary>
    /// Отправить сообщение в чат команды
    /// </summary>
    /// <param name="newTeamChatMessage"></param>
    [HttpPost("send")]
    public Task Send([FromBody] NewTeamChatMessage newTeamChatMessage)
        => GetResult(() =>_teamChatService.SendAsync(AuthorizedUserId, newTeamChatMessage));

    /// <summary>
    /// Получить список сообщений чата команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [HttpPost("{teamId:long}/list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseCollection<TeamChatMessage>))]
    public Task<IActionResult> GetList(
        [FromRoute] long teamId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
        => GetResult(() => _teamChatService.GetListAsync(teamId, offset, limit));
}
