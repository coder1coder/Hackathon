using System;

namespace Hackathon.Chats.Abstractions.Models;

public abstract class BaseChatMessage: BaseNewChatMessage, IChatMessage
{
    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }
    
    /// <summary>
    /// Идентификатор автора сообщения
    /// </summary>
    public long OwnerId { get; set; }
    
    /// <summary>
    /// Дата и время
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Полное имя автора сообщения
    /// </summary>
    public string OwnerFullName { get; set; }
    
    /// <summary>
    /// Полное имя пользователя адресата
    /// </summary>
    public string UserFullName { get; set; }
}
