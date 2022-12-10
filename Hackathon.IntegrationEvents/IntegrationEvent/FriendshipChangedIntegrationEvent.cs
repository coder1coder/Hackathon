using System.Text.Json.Serialization;
using Hackathon.Abstraction.IntegrationEvents;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public class FriendshipChangedIntegrationEvent: IIntegrationEvent
{
    [JsonPropertyName("userIds")]
    public long[] UserIds { get; }

    public FriendshipChangedIntegrationEvent(long[] userIds)
        => UserIds = userIds;
}
