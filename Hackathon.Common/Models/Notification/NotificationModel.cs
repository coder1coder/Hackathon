using System;

namespace Hackathon.Common.Models.Notification;

public class NotificationModel
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public long UserId { get; set; }
    public long? OwnerId { get; set; }
    public string Data { get; set; }
}
