import {Component} from '@angular/core';
import {BaseNotificationComponent} from "./base-notification.component";

@Component({
  selector: 'notification-friendship-request',
  template: `
    <notification-layout [viewModel]="viewModel" (onRemove)="onRemove.emit($event)">
      {{ Notification.getParsedData(notification?.data)?.Message }}

      <ng-container buttons>
        <view-profile-button [userId]="notification.ownerId"></view-profile-button>
        <friendship-offer-button [friendId]="notification.ownerId"></friendship-offer-button>
      </ng-container>
    </notification-layout>
  `
})

export class NotificationFriendshipRequestComponent extends BaseNotificationComponent {
  constructor() {
    super(`supervised_user_circle`, `Запрос дружбы`);
  }
}
