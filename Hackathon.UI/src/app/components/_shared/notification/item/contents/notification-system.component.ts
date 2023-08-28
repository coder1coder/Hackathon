import {Component} from '@angular/core';
import {BaseNotificationComponent} from "./base-notification.component";

@Component({
  selector: 'notification-system',
  template: `
    <notification-layout [viewModel]="viewModel" (onRemove)="onRemove.emit($event)">>

      {{ Notification.getParsedData(notification.data)?.Message }}

    </notification-layout>
  `
})

export class NotificationSystemComponent extends BaseNotificationComponent {
  constructor() {
    super(`message`, `Системное сообщение`);
  }
}
