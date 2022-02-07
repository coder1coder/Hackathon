import {Component} from '@angular/core';
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {NotificationModel} from "../../../models/Notification/NotificationModel";
import {NotificationService} from "../../../services/notification.service";
import {GetFilterModel} from "../../../models/GetFilterModel";
import {NotificationFilterModel} from "../../../models/Notification/NotificationFilterModel";

@Component({
  selector: 'notification-list',
  templateUrl: './notification.list.component.html',
  styleUrls: ['./notification.list.component.scss']
})

export class NotificationListComponent extends BaseTableListComponent<NotificationModel>{

  Notification = Notification;

  override getDisplayColumns(): string[] {
    return ['type', 'ownerId', 'actions'];
  }

  constructor(private notificationService: NotificationService) {
    super(NotificationListComponent.name);
  }

  fetch(){

    let model = new GetFilterModel<NotificationFilterModel>();
    model.Page = this.pageSettings.pageIndex;
    model.PageSize = this.pageSettings.pageSize;

    this.notificationService.getNotifications(model)
      .subscribe({
        next: (r: BaseCollectionModel<NotificationModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  rowClick(notification: NotificationModel){
  }
}
