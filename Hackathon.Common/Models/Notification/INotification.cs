using System;

namespace Hackathon.Common.Models.Notification;

public interface INotification
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
    /// Идентификатор автора уведомления, 0 - уведомление отправлено системой
    /// </summary>
    public long? OwnerId { get; set; }
}
