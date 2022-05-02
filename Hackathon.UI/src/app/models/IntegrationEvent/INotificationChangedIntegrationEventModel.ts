export interface INotificationChangedIntegrationEventModel
{
  notificationIds: string[],
  operation:NotificationChangedOperation
}

export enum NotificationChangedOperation
{
  Created, Updated, Deleted
}
