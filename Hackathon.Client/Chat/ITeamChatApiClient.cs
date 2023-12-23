using Hackathon.Chats.Abstractions.Models.Teams;
using Refit;

namespace Hackathon.Client.Chat;

public interface ITeamChatApiClient
{
    private const string BaseRoute = "/api/chat/team";

    /// <summary>
    /// Отправить сообщение в чат команды
    /// </summary>
    /// <param name="newTeamChatMessage"></param>
    [Post(BaseRoute + "/send")]
    Task SendAsync([Body] NewTeamChatMessage newTeamChatMessage);

    /// <summary>
    /// Получить список сообщений чата команды
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    [Post(BaseRoute + "/{teamId}/list")]
    Task GetListAsync(long teamId, [Query] int offset = 0, [Query] int limit = 300);
}
