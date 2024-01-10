import { Component, Input, OnDestroy } from '@angular/core';
import { Notification } from 'src/app/models/Notification/Notification';
import { NotificationService } from 'src/app/services/notification.service';
import { Subject, takeUntil } from 'rxjs';

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

  constructor(private notificationService: NotificationService) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public remove(event: MouseEvent, ids: string[]): void {
    this.notificationService
      .remove(ids)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        // this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
      });
  }
}
