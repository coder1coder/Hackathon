namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Группа уведомлений
/// </summary>
public enum NotificationGroup: byte
{
    /// <summary>
    /// Система
    /// </summary>
    System = 0,

    /// <summary>
    /// Мероприятия
    /// </summary>
    Events = 1,

    /// <summary>
    /// Команды
    /// </summary>
    Teams = 2,

    /// <summary>
    /// Друзья
    /// </summary>
    Friends = 3
}
