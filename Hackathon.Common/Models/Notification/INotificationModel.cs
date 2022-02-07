using System;

namespace Hackathon.Common.Models.Notification;

public interface INotificationModel
{
    public string Data { get; set; }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}