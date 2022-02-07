import {AfterViewInit, Component} from "@angular/core";
import {NotificationService} from "../../services/notification.service";
import {BaseCollectionModel} from "../../models/BaseCollectionModel";
import {NotificationModel} from "../../models/Notification/NotificationModel";
import {Notification} from "../../models/Notification/Notification";
import {Router} from "@angular/router";
import {NotificationFilterModel} from "../../models/Notification/NotificationFilterModel";
import {GetFilterModel, SortOrder} from "../../models/GetFilterModel";

@Component({
  selector: 'notification-bell',
  templateUrl: './notification-bell.component.html',
  styleUrls: ['./notification-bell.component.scss'],
})

export class NotificationBellComponent implements AfterViewInit
{
  Notification = Notification
  notifications:BaseCollectionModel<NotificationModel> = new BaseCollectionModel<NotificationModel>();
  unreadNotificationsCount:number = 0;

  constructor(private notificationService: NotificationService, private router: Router) {
  }

  ngAfterViewInit(): void {
    this.notificationService.onPublished = _ => {
      this.updateUnreadNotificationsCount();
      this.fetch();
    };

    this.updateUnreadNotificationsCount();
    this.fetch();
  }

  markAllAsRead(){
    this.notificationService
      .markAsRead([])
      .subscribe(_=> this.updateUnreadNotificationsCount());
  }

  updateUnreadNotificationsCount(){
    this.notificationService
      .getUnreadNotificationsCount()
      .subscribe(x=>this.unreadNotificationsCount = x);
  }

  fetch(){
    let filter = new GetFilterModel<NotificationFilterModel>();
    filter.SortOrder = SortOrder.Desc;
    filter.SortBy = "createdAt";

    this.notificationService
      .getNotifications(filter)
      .subscribe(x=> this.notifications = x);
  }

  viewAll(){
    this.router.navigate(['notifications']);
  }

  menuOpened(){
    if (this.unreadNotificationsCount > 0)
      this.markAllAsRead();
  }

}
