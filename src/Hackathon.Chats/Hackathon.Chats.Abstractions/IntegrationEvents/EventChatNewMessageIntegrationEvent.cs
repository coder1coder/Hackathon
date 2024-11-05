using System;

namespace Hackathon.Chats.Abstractions.IntegrationEvents;

public class EventChatNewMessageIntegrationEvent: IEventChatIntegrationEvent
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }

    public string GetTopicName() => ChatsTopicNames.EventChatNewMessage;
}
