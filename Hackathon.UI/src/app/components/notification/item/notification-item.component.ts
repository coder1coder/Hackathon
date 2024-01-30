import { Component, Input, OnDestroy } from '@angular/core';
import { Notification } from 'src/app/models/Notification/Notification';
import { Subject, takeUntil } from 'rxjs';
import { NotificationsClient } from 'src/app/clients/notifications.client';

@Component({
  selector: `notification-item`,
  templateUrl: `notification-item.component.html`,
  styleUrls: [`notification-item.component.scss`],
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
