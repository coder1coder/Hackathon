using System;
using Hackathon.Informing.Abstractions.Models;

namespace Hackathon.Informing.DAL.Entities;

/// <summary>
/// Уведомление
/// </summary>
public class NotificationEntity
{
    /// <summary>
    /// Идентификатор уведомления
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Тип уведомления
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Идентификатор пользователя-получателя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор пользователя-отправителя
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Дата и время создания уведомления
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Сообщение является прочтенным
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// Дополнительная информация
    /// </summary>
    public string Data { get; set; }
}
