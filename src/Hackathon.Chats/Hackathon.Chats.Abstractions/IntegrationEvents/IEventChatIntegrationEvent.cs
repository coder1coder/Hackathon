using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public interface IEventChatIntegrationEvent: IIntegrationEvent
{
    long EventId { get; }
}
