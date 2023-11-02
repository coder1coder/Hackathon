namespace Hackathon.Informing.Abstractions.Models;

/// <summary>
/// Тип уведомления
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Системное уведомление
    /// </summary>
    System = 0,

    /// <summary>
    /// Приглашение в команду
    /// </summary>
    TeamInvite = 1,

    /// <summary>
    /// Приглашение на мероприятие
    /// </summary>
    EventInvite = 2
}
