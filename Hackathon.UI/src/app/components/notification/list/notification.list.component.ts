import {Component} from '@angular/core';
import {BaseCollection} from "../../../models/BaseCollection";
import {NotificationService} from "../../../services/notification.service";
import {GetListParameters} from "../../../models/GetListParameters";
import {NotificationFilter} from "../../../models/Notification/NotificationFilter";
import {Notification} from "../../../models/Notification/Notification";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss']
})

export class NotificationListComponent extends BaseTableListComponent<Notification>{

  Notification = Notification;

  constructor(private notificationService: NotificationService, private authService: AuthService) {
    super(NotificationListComponent.name);
  }

  override ngAfterViewInit() {

    this.notificationService.onChanged = _ => {
      if (this.authService.isLoggedIn())
        this.fetch();
    }

    super.ngAfterViewInit();
  }

  override fetch() {
    let model = new GetListParameters<NotificationFilter>();

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

  rowClick(notification: Notification) {}

  public remove(event:MouseEvent, ids:string[]) {
    event.stopPropagation();
    this.notificationService.remove(ids).subscribe(_=>{
      this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
    });
  }

  public getDisplayColumns(): string[] {
    return ['notify', 'type', 'createdAt', 'actions'];
  }

  public removeAll(event:MouseEvent) {
    let ids = this.items.map(x => x.id!);
    this.remove(event, ids);
  }

  public get isNotificationsExists() : boolean {
    return  this.items.length === 0;
  }
}
