import {AfterViewInit, Component} from '@angular/core';
import {BaseCollection} from "../../../models/BaseCollection";
import {NotificationService} from "../../../services/notification.service";
import {GetListParameters} from "../../../models/GetListParameters";
import {NotificationFilter} from "../../../models/notifications/NotificationFilter";
import {Notification} from "../../../models/notifications/Notification";
import {AuthService} from "../../../services/auth.service";
import {NotificationType} from "../../../models/notifications/NotificationType";
import {PageResultComponent} from "../../page-result.component";
import {NotificationGroup} from "../../../models/notifications/notification-group";

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss']
})

export class NotificationListComponent extends PageResultComponent<Notification> implements AfterViewInit{

  Notification = Notification;
  NotificationType = NotificationType;
  NotificationGroup = NotificationGroup;

  byNotificationGroupFilter?: NotificationGroup;

  constructor(private notificationService: NotificationService, private authService: AuthService) {
    super(NotificationListComponent.name);
  }

  override ngAfterViewInit() {
    super.ngAfterViewInit()

    this.notificationService.onChanged = _ => {
      if (this.authService.isLoggedIn())
        this.fetch();
    }

  }

  override fetch() {
    let model = new GetListParameters<NotificationFilter>();

    model.Filter = new NotificationFilter();
    model.Filter.group = this.byNotificationGroupFilter;
    model.Offset = this.pageSettings.pageIndex;
    model.Limit = this.pageSettings.pageSize;

    this.notificationService.getNotifications(model)
      .subscribe({
        next: (r: BaseCollection<Notification>) => {
          this.items = r.items
          this.pageSettings.length = r.totalCount
        }
      });
  }

  rowClick(notification: Notification){
    this.notificationService.markAsRead([notification.id!]).subscribe({
      next:()=>{
        this.items.filter(s=>s.id == notification.id).forEach(s=>s.isRead = true);
      }
    })
  }

  public onNotificationRemoved(id:string) {
    this.excludeItems([id])
  }

  public removeAll() {
    let ids = this.items.map(x => x.id!);
    this.notificationService.remove(ids).subscribe(_=>{
      this.excludeItems(ids)
    });
  }

  public get isNotificationsExists() : boolean {
    return this.items.length === 0;
  }

  public setByGroupFilter(group: NotificationGroup): void{
    this.byNotificationGroupFilter = group;
    this.fetch();
  }

  public resetByGroupFilter(): void{
    this.byNotificationGroupFilter = undefined;
    this.fetch();
  }

  private excludeItems(ids:string[]): void {
    this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
  }
}
