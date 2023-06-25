import {
  Component, EventEmitter,
  Input, Output,
} from '@angular/core';
import {NotificationType} from 'src/app/models/notifications/NotificationType';
import {Notification} from "../../../models/notifications/Notification";
import {NotificationService} from "../../../services/notification.service";

@Component({
  selector: 'notification-item',
  template: `
    <ng-container [ngSwitch]="notification.type">

      <notification-friendship-request
        *ngSwitchCase="NotificationType.FriendshipRequest"
        [notification]="notification"
        (onRemove)="remove($event)"
      ></notification-friendship-request>

      <notification-friendship-accepted
        *ngSwitchCase="NotificationType.FriendshipAccepted"
        [notification]="notification"
        (onRemove)="remove($event)"
      ></notification-friendship-accepted>

      <notification-friendship-rejected
      *ngSwitchCase="NotificationType.FriendshipRejected"
      [notification]="notification"
      (onRemove)="remove($event)"
      ></notification-friendship-rejected>

      <notification-system
        *ngSwitchDefault
        [notification]="notification"
        (onRemove)="remove($event)">
      </notification-system>

    </ng-container>
  `
})

export class NotificationItemComponent{

  @Input() notification: Notification;
  @Output() onRemoved = new EventEmitter<string>();

  NotificationType = NotificationType;

  constructor(private notificationService: NotificationService) {
  }

  public remove(id:string | undefined) {
    if (id == undefined)
      return;

    this.notificationService
      .remove([id])
      .subscribe({next: _ => this.onRemoved.emit(id) });
  }
}
