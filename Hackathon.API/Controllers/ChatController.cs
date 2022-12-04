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
    private readonly IChatNotifyService _chatNotifyService;

    public ChatController(IChatService chatService,
        IChatNotifyService chatNotifyService)
    {
        _chatService = chatService;
        _chatNotifyService = chatNotifyService;
    }

    [HttpPost("team/{teamId:long}/send")]
    public async Task SendTeamMessage(CreateTeamChatMessage createTeamChatMessage)
    {
        createTeamChatMessage.OwnerId = UserId;
        await _chatService.SendMessage(createTeamChatMessage);

        //TODO: добавить проверку что пользователь является овнером команды
        if (createTeamChatMessage.Type == ChatMessageType.WithNotify)
           await _chatNotifyService.SendMessage(createTeamChatMessage);
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
