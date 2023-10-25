namespace Hackathon.Chats.Abstractions.Models;

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
    /// Опции сообщения (возможно несколько значений)
    /// </summary>
    ChatMessageOption Options { get; set; }
}
