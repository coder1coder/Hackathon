namespace Hackathon.Common.Models.Notification;

/// <summary>
/// Фильтр списка уведомлений
/// </summary>
public class NotificationFilterModel
{
    /// <summary>
    /// Признак прочтения
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>
    /// Группа уведомлений
    /// </summary>
    public NotificationGroup? Group { get; set; }
}
