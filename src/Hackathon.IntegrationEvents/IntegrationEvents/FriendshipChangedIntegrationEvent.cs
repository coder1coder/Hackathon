using System.Text.Json.Serialization;
using Hackathon.Common.Abstraction.IntegrationEvents;
using Hackathon.IntegrationEvents.Topics;

namespace Hackathon.IntegrationEvents.IntegrationEvents;

public sealed class FriendshipChangedIntegrationEvent: IIntegrationEvent
{
    [JsonPropertyName("userIds")]
    public long[] UserIds { get; }

    public FriendshipChangedIntegrationEvent(long[] userIds)
        => UserIds = userIds;

    public string GetTopicName() => TopicNames.FriendshipChanged;
}
