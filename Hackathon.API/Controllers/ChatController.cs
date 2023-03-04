using System.Threading.Tasks;
using Hackathon.Abstraction.Chat;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

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
        => _chatService.SendMessage<TeamChatMessage>(UserId, createTeamChatMessage);

    [HttpPost("team/{teamId:long}/list")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(BaseCollectionResponse<TeamChatMessage>))]
    public async Task<IActionResult> GetTeamMessages(
        [FromRoute] long teamId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
    {
        var result = await _chatService.GetTeamMessages(teamId, offset, limit);

        if (!result.IsSuccess)
            return await GetResult(() => Task.FromResult(result));

        return Ok(new BaseCollectionResponse<TeamChatMessage>
        {
            Items = result.Data.Items,
            TotalCount = result.Data.TotalCount
        });
    }
}
