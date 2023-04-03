namespace Hackathon.Common.Models.Chat;

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
    /// Контекст
    /// </summary>
    public ChatMessageType Type { get; }

    /// <summary>
    /// Опции сообщения
    /// </summary>
    public ChatMessageOption Options { get; set; }

    protected BaseNewChatMessage(ChatMessageType chatMessageType)
    {
        Type = chatMessageType;
    }
}
