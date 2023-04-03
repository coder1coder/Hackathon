namespace Hackathon.Common.Models.Chat;

public interface INewChatMessage
{
    /// <summary>
    /// Идентификатор пользователя кому адресовано сообщение
    /// </summary>
    long? UserId { get; set; }

    /// <summary>
    /// Текст сообщения
    /// </summary>
    string Message { get; set; }

    /// <summary>
    /// Контекст сообщения
    /// </summary>
    ChatMessageType Type { get; }

    /// <summary>
    /// Опции сообщения (возможно несколько значений)
    /// </summary>
    ChatMessageOption Options { get; set; }
}
