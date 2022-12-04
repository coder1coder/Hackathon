using Hackathon.Common.Models.Chat;

namespace Hackathon.Abstraction.Chat;

public interface IChatNotifyService
{
    /// <summary>
    /// Отправить уведомления команде
    /// </summary>
    /// <param name="createChatMessage">Сообщение</param>
    /// <returns></returns>
    Task SendMessage(CreateTeamChatMessage createTeamChatMessage);
}
