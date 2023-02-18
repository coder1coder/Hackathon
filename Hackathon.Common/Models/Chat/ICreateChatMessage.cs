using System;

namespace Hackathon.Common.Models.Chat;

public interface ICreateChatMessage
{
    /// <summary>
    /// Идентификатор автора сообщения
    /// </summary>
    long OwnerId { get; set; }

    /// <summary>
    /// Идентификатор пользователя кому адресовано сообщение
    /// </summary>
    long? UserId { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Дата и время создания сообщения
    /// </summary>
    DateTime Timestamp { get; set; }

    /// <summary>
    /// Контекст сообщения
    /// </summary>
    ChatMessageType Type { get; set; }

    /// <summary>
    /// Опции сообщения (возможно несколько значений)
    /// </summary>
    ChatMessageOption Options { get; set; }
}
