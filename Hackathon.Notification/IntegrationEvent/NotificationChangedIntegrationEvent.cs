using System;

namespace Hackathon.Notification.IntegrationEvent;

public class NotificationChangedIntegrationEvent: IIntegrationEvent
{
    public Guid[] NotificationIds { get; }
    public NotificationChangedOperation Operation { get; }

    public NotificationChangedIntegrationEvent(NotificationChangedOperation operation, Guid[] notificationIds)
    {
        NotificationIds = notificationIds;
        Operation = operation;
    }
}

public enum NotificationChangedOperation
{
    Created,
    Updated,
    Deleted
}