import { Component, OnDestroy, OnInit } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { NotificationFilter } from '../../../models/Notification/NotificationFilter';
import { GetListParameters, SortOrder } from '../../../models/GetListParameters';
import { Notification } from '../../../models/Notification/Notification';
import { RouterService } from '../../../services/router.service';
import { AuthService } from 'src/app/services/auth.service';
import { SignalRService } from '../../../services/signalr.service';
import { Subject, takeUntil } from 'rxjs';
import { NotificationsClient } from 'src/app/clients/notifications.client';
import { MatIconButton, MatButton } from '@angular/material/button';
import { MatMenuTrigger, MatMenu } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatBadge } from '@angular/material/badge';

import { NotificationItemComponent } from '../item/notification-item.component';
import { MatList, MatListItem } from '@angular/material/list';

@Component({
    selector: 'notification-bell',
    templateUrl: './notification.bell.component.html',
    styleUrls: ['./notification.bell.component.scss'],
    imports: [MatIconButton, MatMenuTrigger, MatIcon, MatBadge, MatMenu, NotificationItemComponent, MatButton, MatList, MatListItem]
})
export class NotificationBellComponent implements OnInit, OnDestroy {
  public notifications: BaseCollection<Notification> = new BaseCollection<Notification>();
  public unreadNotificationsCount: number = 0;
  public notificationLimit: number = 3;

  private destroy$ = new Subject();

  constructor(
    private notificationsClient: NotificationsClient,
    private signalRService: SignalRService,
    private router: RouterService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.signalRService.onNotificationChanged = (): void => {
      if (this.authService.isLoggedIn()) {
        this.updateUnreadNotificationsCount();
        this.fetch();
      }
    };

    this.updateUnreadNotificationsCount();
    this.fetch();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public viewAll = (): Promise<boolean> => this.router.Notifications.List();

  public menuOpened(): void {
    if (this.unreadNotificationsCount > 0) this.markAllAsRead();
  }

  private markAllAsRead(): void {
    this.notificationsClient
      .markAsRead([])
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.updateUnreadNotificationsCount());
  }

  private updateUnreadNotificationsCount(): void {
    this.notificationsClient
      .getUnreadNotificationsCount()
      .pipe(takeUntil(this.destroy$))
      .subscribe((res: number) => (this.unreadNotificationsCount = res));
  }

  private fetch(): void {
    const filter: GetListParameters<NotificationFilter> =
      new GetListParameters<NotificationFilter>();
    filter.SortOrder = SortOrder.Desc;
    filter.SortBy = 'createdAt';
    filter.Limit = this.notificationLimit;

    this.notificationsClient
      .getNotifications(filter)
      .pipe(takeUntil(this.destroy$))
      .subscribe((res: BaseCollection<Notification>) => (this.notifications = res));
  }

  private remove(event: MouseEvent, ids: string[]): void {
    event.stopPropagation();
    this.notificationsClient
      .remove(ids)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.notifications.items = this.notifications.items.filter(
          //TODO: remove type assertioon
          (notification: Notification) => !ids.includes(notification?.id as string),
        );
        this.notifications.totalCount = this.notifications.totalCount - ids.length;
      });
  }
}
