export interface NotificationChangedIntegrationEventModel
{
  notificationIds: string[],
  operation:NotificationChangedOperation
}

export enum NotificationChangedOperation
{
  Created, Updated, Deleted
}
