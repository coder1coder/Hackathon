using System;
using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public class EventChatNewMessageIntegrationEvent: IIntegrationEvent
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }
}
