import {Component} from '@angular/core';
import {BaseNotificationComponent} from "./base-notification.component";

@Component({
  selector: 'notification-friendship-rejected',
  template: `
    <notification-layout [viewModel]="viewModel" (onRemove)="onRemove.emit($event)">
      {{ Notification.getParsedData(notification?.data)?.Message }}

      <ng-container buttons>
        <view-profile-button [userId]="notification.ownerId"></view-profile-button>
      </ng-container>
    </notification-layout>
  `
})

export class NotificationFriendshipRejectedComponent extends BaseNotificationComponent {
  constructor() {
    super(`supervised_user_circle`, `Запрос дружбы отклонен`);
  }
}
