using System.Threading.Tasks;
using Hackathon.Abstraction.Chat;
using Hackathon.Common.Models.Chat;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    public async Task SendTeamMessage([FromBody] CreateTeamChatMessage createTeamChatMessage)
    {
        createTeamChatMessage.OwnerId = UserId;
        await _chatService.SendMessage(createTeamChatMessage);
    }

    [HttpPost("team/{teamId:long}/list")]
    public async Task<BaseCollectionResponse<TeamChatMessage>> GetTeamMessages(
        [FromRoute] long teamId,
        [FromQuery] int offset = 0,
        [FromQuery] int limit = 300)
    {
        var collection = await _chatService.GetTeamMessages(teamId, offset, limit);
        return new BaseCollectionResponse<TeamChatMessage>
        {
            Items = collection.Items,
            TotalCount = collection.TotalCount
        };
    }
}
