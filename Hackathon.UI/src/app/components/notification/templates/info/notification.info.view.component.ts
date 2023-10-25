import { Component, Input } from '@angular/core';
import { Notification } from "../../../../models/Notification/Notification";

@Component({
  selector: 'notification-info-view',
  templateUrl: './notification.info.view.component.html',
  styleUrls: ['./notification.info.view.component.scss'],
})

export class NotificationInfoViewComponent{
  notification = Notification;
  @Input() notify: Notification | undefined;
  @Input() hideDate: boolean = false;
}
