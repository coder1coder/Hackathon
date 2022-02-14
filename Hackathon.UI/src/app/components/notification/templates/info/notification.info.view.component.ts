import {Component, Input} from '@angular/core';
import {NotificationModel} from "../../../../models/Notification/NotificationModel";
import {Notification} from "../../../../models/Notification/Notification";

@Component({
  selector: 'notification-info-view',
  templateUrl: './notification.info.view.component.html',
  styleUrls: ['./notification.info.view.component.scss']
})

export class NotificationInfoViewComponent{

  Notification = Notification;
  @Input() notify: NotificationModel | undefined;
  @Input() hideDate: boolean = false;

  constructor() {
  }
}
