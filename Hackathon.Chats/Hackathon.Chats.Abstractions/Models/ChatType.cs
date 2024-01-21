namespace Hackathon.Chats.Abstractions.Models;

/// <summary>
/// Тип чата
/// </summary>
public enum ChatType: byte
{
    /// <summary>
    /// Чат команды
    /// </summary>
    TeamChat = 0,
    
    /// <summary>
    /// Чат мероприятия
    /// </summary>
    EventChat = 1
}
