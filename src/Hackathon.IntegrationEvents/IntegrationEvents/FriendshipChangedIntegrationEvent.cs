using System.Text.Json.Serialization;
using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.IntegrationEvents.IntegrationEvents;

public sealed class FriendshipChangedIntegrationEvent: IIntegrationEvent
{
    [JsonPropertyName("userIds")]
    public long[] UserIds { get; }

    public FriendshipChangedIntegrationEvent(long[] userIds)
        => UserIds = userIds;
}
