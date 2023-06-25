using System;

namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Уведомление
/// </summary>
public class NotificationModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Тип
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Группа
    /// </summary>
    public NotificationGroup Group { get; set; }

    /// <summary>
    /// Дата и время создания
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Признак прочтения
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Идентификатор пользователя которому адресовано уведомление
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор пользователя автора уведомления
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public string Data { get; set; }
}
