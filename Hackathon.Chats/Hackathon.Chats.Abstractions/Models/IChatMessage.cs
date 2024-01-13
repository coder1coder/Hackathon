using System;

namespace Hackathon.Chats.Abstractions.Models;

public interface IChatMessage: INewChatMessage
{
    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// Идентификатор автора сообщения
    /// </summary>
    long OwnerId { get; set; }

    /// <summary>
    /// Дата и время создания сообщения
    /// </summary>
    DateTime Timestamp { get; set; }

    /// <summary>
    /// Полное имя автора сообщения
    /// </summary>
    string OwnerFullName { get; set; }

    /// <summary>
    /// Полное имя адресата
    /// </summary>
    string UserFullName { get; set; }
}
