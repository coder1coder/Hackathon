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
import { DefaultLayoutComponent } from '../../layouts/default/default.layout.component';
import { NgIf, NgFor } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { NotificationItemComponent } from '../item/notification-item.component';
import { MatCardContent, MatCardFooter } from '@angular/material/card';
import { MatPaginator } from '@angular/material/paginator';
import { MatList, MatListItem } from '@angular/material/list';

@Component({
    selector: 'notification-list',
    templateUrl: './notification.list.component.html',
    styleUrls: ['./notification.list.component.scss'],
    imports: [
        DefaultLayoutComponent,
        NgIf,
        MatButton,
        MatIcon,
        NgFor,
        NotificationItemComponent,
        MatCardContent,
        MatCardFooter,
        MatPaginator,
        MatList,
        MatListItem,
    ],
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
    //TODO: remove type assertioon
    const ids: string[] = this.items.map(
      (notification: Notification) => notification.id,
    ) as string[];
    this.remove(event, ids);
  }

  public rowClick(): void {
    /* unused */
  }
}
