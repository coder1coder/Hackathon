import { Component, OnInit } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { GetListParameters } from '../../../models/GetListParameters';
import { NotificationFilter } from '../../../models/Notification/NotificationFilter';
import { Notification } from '../../../models/Notification/Notification';
import { BaseTableListComponent } from '../../../common/base-components/base-table-list.component';
import { AuthService } from '../../../services/auth.service';
import { takeUntil } from 'rxjs';
import { SignalRService } from '../../../services/signalr.service';
import { NotificationsClient } from 'src/app/clients/notifications.client';

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss'],
})
export class NotificationListComponent
  extends BaseTableListComponent<Notification>
  implements OnInit
{
  constructor(
    private notificationsClient: NotificationsClient,
    private signalRService: SignalRService,
    private authService: AuthService,
  ) {
    super(NotificationListComponent.name);
  }

  public ngOnInit(): void {
    super.ngOnInit();

    this.signalRService.onNotificationChanged = (): void => {
      if (this.authService.isLoggedIn()) {
        this.fetch();
      }
    };
  }

  override fetch(): void {
    const model: GetListParameters<NotificationFilter> =
      new GetListParameters<NotificationFilter>();
    model.Offset = this.pageSettings.pageIndex;
    model.Limit = this.pageSettings.pageSize;

    this.notificationsClient
      .getNotifications(model)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<Notification>) => {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
      });
  }

  public getDisplayColumns(): string[] {
    return [];
  }

  public remove(event: MouseEvent, ids: string[]): void {
    event.stopPropagation();
    this.notificationsClient
      .remove(ids)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.items = this.items.filter((x) => x.id !== undefined && !ids.includes(x.id));
      });
  }

  public removeAll(event: MouseEvent): void {
    const ids: string[] = this.items.map((notification: Notification) => notification.id);
    this.remove(event, ids);
  }

  public rowClick(): void {
    /* unused */
  }
}
