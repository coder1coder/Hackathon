using System;
using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public sealed class TeamChatNewMessageIntegrationEvent: IIntegrationEvent
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }
}
