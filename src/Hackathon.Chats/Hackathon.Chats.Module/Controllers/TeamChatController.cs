using System;
using System.Net;
using System.Threading.Tasks;
using Hackathon.API.Module;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.Abstractions.Services;
using Hackathon.Common.Models.Base;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Hackathon.Chats.Module.Controllers;

[Route("api/chat/team")]
[SwaggerTag("Чат команды")]
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
    /// Получить сообщение чата команды по идентификатору
    /// </summary>
    [HttpGet("messages/{messageId:guid}")]
    public Task<IActionResult> GetTeamChatMessage(Guid messageId)
        => GetResult(() => _teamChatService.GetAsync(AuthorizedUserId, messageId));

    /// <summary>
    /// Получить список сообщений чата команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    [HttpPost("{teamId:long}/list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseCollection<TeamChatMessage>))]
    public Task<IActionResult> GetList(
        [FromRoute] long teamId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
        => GetResult(() => _teamChatService.GetListAsync(teamId, offset, limit));
}
