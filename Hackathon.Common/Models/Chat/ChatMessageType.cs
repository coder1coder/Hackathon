namespace Hackathon.Common.Models.Chat;

/// <summary>
/// Тип сообщения
/// </summary>
public enum ChatMessageType: byte
{
    /// <summary>
    /// Сообщение чата команды
    /// </summary>
    TeamChat = 0,

    /// <summary>
    /// Общий чат события
    /// </summary>
    EventChat = 1
}
