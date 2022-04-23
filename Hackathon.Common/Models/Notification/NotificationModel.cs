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

/// <summary>
/// Тип уведомления
/// </summary>
public enum NotificationType
{
    Information = 0,
    TeamInvite = 1,
    EventInvite = 2,
}