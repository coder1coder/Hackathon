using System;
using Hackathon.Chats.Abstractions.Models;

namespace Hackathon.Chats.DAL.Entities;

public class ChatMessageEntity
{
    /// <summary>
    /// Идентификатор сообщения
    /// </summary>
    public Guid MessageId { get; set; }

    /// <summary>
    /// Идентификатор чата
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Тип чата
    /// </summary>
    public ChatType ChatType { get; set; }

    /// <summary>
    /// Автор сообщения
    /// <remarks>NULL - Система</remarks>
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Полное имя автора
    /// </summary>
    public string OwnerFullName { get; set; }

    /// <summary>
    /// Идентификатор пользователя, адресат
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Полное имя адресата
    /// </summary>
    public string UserFullName { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Опции сообщения
    /// </summary>
    public ChatMessageOption Options { get; set; }

    /// <summary>
    /// Дата и время сообщения
    /// </summary>
    public DateTime Timestamp { get; set; }
}
