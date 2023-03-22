using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Team;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace Hackathon.API.Controllers;

[SwaggerTag("Чат")]
public class ChatController: BaseController
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Отправить сообщение в чат команды
    /// </summary>
    /// <param name="createTeamChatMessage"></param>
    [HttpPost("team/send")]
    public Task SendTeamMessage([FromBody] CreateTeamChatMessage createTeamChatMessage)
        => _chatService.SendMessage<TeamChatMessage>(AuthorizedUserId, createTeamChatMessage);

    [HttpPost("team/{teamId:long}/list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseCollection<TeamChatMessage>))]
    public async Task<IActionResult> GetTeamMessages(
        [FromRoute] long teamId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
    {
        var result = await _chatService.GetTeamMessages(teamId, offset, limit);

        if (!result.IsSuccess)
            return await GetResult(() => Task.FromResult(result));

        return Ok(new BaseCollection<TeamChatMessage>
        {
            Items = result.Data.Items,
            TotalCount = result.Data.TotalCount
        });
    }
}
