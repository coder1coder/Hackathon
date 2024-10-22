namespace Hackathon.Chats.Abstractions.Models;

/// <summary>
/// Базовые параметры сообщения чата
/// </summary>
public abstract class BaseNewChatMessage: INewChatMessage
{
    /// <summary>
    /// Идентификатор пользователя, адресат
    /// </summary>
    public long? UserId { get; set; }

    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Опции сообщения
    /// </summary>
    public ChatMessageOption Options { get; set; }
}
