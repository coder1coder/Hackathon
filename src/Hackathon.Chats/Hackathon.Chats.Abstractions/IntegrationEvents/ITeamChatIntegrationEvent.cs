using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public interface ITeamChatIntegrationEvent: IIntegrationEvent
{
    long TeamId { get; }
}
