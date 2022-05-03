export interface INotificationChangedIntegrationEvent
{
  notificationIds: string[],
  operation:NotificationChangedOperation
}

export enum NotificationChangedOperation
{
  Created, Updated, Deleted
}
