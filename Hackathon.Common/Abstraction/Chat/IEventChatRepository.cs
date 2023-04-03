using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat.Event;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Chat;

public interface IEventChatRepository: IChatRepository<EventChatMessage>
{
    Task<BaseCollection<EventChatMessage>> GetMessagesAsync(long eventId, int offset = 0, int limit = 300);
}
