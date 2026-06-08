import { Component, Input, OnDestroy } from '@angular/core';
import { Notification } from 'src/app/models/Notification/Notification';
import { Subject, takeUntil } from 'rxjs';
import { NotificationsClient } from 'src/app/clients/notifications.client';
import { DatePipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { NotificationInfoViewComponent } from '../templates/info/notification.info.view.component';
import { NotificationTeamJoinRequestDecisionViewComponent } from '../templates/teams/teamJoinRequestDecision/notification.teamJoinRequestDecision.view.component';
import { MatIconButton } from '@angular/material/button';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';

@Component({
    selector: `notification-item`,
    templateUrl: `notification-item.component.html`,
    styleUrls: [`notification-item.component.scss`],
    imports: [MatIcon, NotificationInfoViewComponent, NotificationTeamJoinRequestDecisionViewComponent, MatIconButton, MatMenuTrigger, MatMenu, MatMenuItem, DatePipe]
})
export class NotificationItemComponent implements OnDestroy {
  Notification = Notification;

  @Input() notification: Notification | undefined;
  @Input() hideActions: boolean = true;
  @Input() shortView: boolean = false;

  private destroy$ = new Subject();

  constructor(private notificationsClient: NotificationsClient) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public remove(event: MouseEvent, ids: string[]): void {
    this.notificationsClient
      .remove(ids)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        // this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
      });
  }
}
