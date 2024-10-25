using System;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

/// <summary>
/// Новое сообщение чата команды
/// </summary>
public sealed record TeamChatNewMessageIntegrationEvent: ITeamChatIntegrationEvent
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; }

    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; }

    public TeamChatNewMessageIntegrationEvent(long teamId, Guid messageId)
    {
        TeamId = teamId;
        MessageId = messageId;
    }
}
