using System;
using Hackathon.Abstraction.IntegrationEvents;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

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