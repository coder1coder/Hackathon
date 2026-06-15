import { Component, Input } from '@angular/core';
import { NOTIFICATION_DATETIME_FORMAT } from 'src/app/common/consts/date-formats';
import { ISystemNotificationData } from 'src/app/models/Notification/data/ISystemNotificationData';
import { Notification } from '../../../../models/Notification/Notification';
import { DatePipe } from '@angular/common';
import { MatLine } from '@angular/material/grid-list';

@Component({
    selector: 'notification-info-view',
    templateUrl: './notification.info.view.component.html',
    styleUrls: ['./notification.info.view.component.scss'],
    imports: [
    MatLine,
    DatePipe
],
})
export class NotificationInfoViewComponent {
  notification = Notification;
  NOTIFICATION_DATETIME_FORMAT = NOTIFICATION_DATETIME_FORMAT;
  @Input() notify: Notification | undefined;
  @Input() hideDate: boolean = false;

  //TODO: remove type assertioon
  getMessage(): string {
    return this.notification.getParsedData<ISystemNotificationData>(this.notify?.data as string)
      ?.Message as string;
  }
}
