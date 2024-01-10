export interface INotificationChangedIntegrationEvent {
  notificationIds: string[];
  operation: NotificationChangedOperation;
}

export enum NotificationChangedOperation {
  Created = 0,
  Updated = 1,
  Deleted = 2,
}
