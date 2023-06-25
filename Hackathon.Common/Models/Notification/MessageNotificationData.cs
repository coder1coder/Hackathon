namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Информационное уведомление
/// </summary>
public class MessageNotificationData: INotificationData
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public string Message { get; set; }
}
