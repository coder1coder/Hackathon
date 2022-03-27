import {Component} from '@angular/core';
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {NotificationModel} from "../../../models/Notification/NotificationModel";
import {NotificationService} from "../../../services/notification.service";
import {GetFilterModel} from "../../../models/GetFilterModel";
import {NotificationFilterModel} from "../../../models/Notification/NotificationFilterModel";
import {Notification} from "../../../models/Notification/Notification";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss']
})

export class NotificationListComponent extends BaseTableListComponent<NotificationModel>{

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
    let model = new GetFilterModel<NotificationFilterModel>();

    model.Page = this.pageSettings.pageIndex;
    model.PageSize = this.pageSettings.pageSize;

    this.notificationService.getNotifications(model)
      .subscribe({
        next: (r: BaseCollectionModel<NotificationModel>) => {
          this.items = r.items
          this.pageSettings.length = r.totalCount
        }
      });
  }

  rowClick(notification: NotificationModel) {}

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
    return  this.items.length === 0 ? true : false;
  }
}
