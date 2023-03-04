using Hackathon.Common.Models.Notification;
using System;

namespace Hackathon.DAL.Entities;

public class NotificationEntity
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public long UserId { get; set; }
    public long OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Data { get; set; }
}
