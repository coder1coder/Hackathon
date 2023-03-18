using System;
using Hackathon.Common.Abstraction.IntegrationEvents;

namespace Hackathon.IntegrationEvents.IntegrationEvent;

public sealed class NotificationChangedIntegrationEvent: IIntegrationEvent
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
