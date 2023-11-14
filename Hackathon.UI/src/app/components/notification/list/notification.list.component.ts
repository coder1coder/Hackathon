import { Component } from '@angular/core';
import { BaseCollection } from "../../../models/BaseCollection";
import { NotificationService } from "../../../services/notification.service";
import { GetListParameters } from "../../../models/GetListParameters";
import { NotificationFilter } from "../../../models/Notification/NotificationFilter";
import { Notification } from "../../../models/Notification/Notification";
import { BaseTableListComponent } from "../../../common/base-components/base-table-list.component";
import { AuthService } from "../../../services/auth.service";
import { takeUntil } from "rxjs";
import {SignalRService} from "../../../services/signalr.service";

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss'],
})

export class NotificationListComponent extends BaseTableListComponent<Notification>{

  constructor(
    private notificationService: NotificationService,
    private signalRService: SignalRService,
    private authService: AuthService
  ) {
    super(NotificationListComponent.name);
  }

  override ngOnInit(): void {
    super.ngOnInit();

    this.signalRService.onNotificationChanged = () => {
      if (this.authService.isLoggedIn()) {
        this.fetch();
      }
    }
  }

  override fetch(): void {
    const model = new GetListParameters<NotificationFilter>();
    model.Offset = this.pageSettings.pageIndex;
    model.Limit = this.pageSettings.pageSize;

    this.notificationService.getNotifications(model)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<Notification>) => {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        }
      });
  }

  public getDisplayColumns(): string[] {
    return [];
  }

  public remove(event: MouseEvent, ids: string[]): void {
    event.stopPropagation();
    this.notificationService.remove(ids)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
      });
  }

  public removeAll(event: MouseEvent): void {
    const ids = this.items.map((notification: Notification) => notification.id);
    this.remove(event, ids);
  }

  public rowClick(notification: Notification): void {/* unused */}
}
