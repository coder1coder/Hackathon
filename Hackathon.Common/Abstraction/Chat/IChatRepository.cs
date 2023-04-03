using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface IChatRepository<TChatMessage> where TChatMessage : class, IChatMessage
{
    /// <summary>
    /// Добавить сообщение
    /// </summary>
    /// <param name="chatMessage"></param>
    /// <returns></returns>
    Task AddMessageAsync(TChatMessage chatMessage);

    /// <summary>
    /// Получить список сообщений
    /// </summary>
    /// <param name="key">Уникальный ключ</param>
    /// <param name="offset">Смещение</param>
    /// <param name="limit">Лимит</param>
    /// <returns></returns>
    Task<BaseCollection<TChatMessage>> GetMessagesByKeyAsync(string key, int offset = 0, int limit = 300);
}
