namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Параметры создания нового уведомления
/// </summary>
/// <typeparam name="T"></typeparam>
public class CreateNotificationModel<T> where T: class
{
    /// <summary>
    /// Тип уведомления
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Получатель
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Отправитель
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Дополнительные сведения
    /// </summary>
    public T Data { get; set; }
}
