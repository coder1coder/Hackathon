import {AfterViewInit, Component} from "@angular/core";
import {BaseCollection} from "../../../../models/BaseCollection";
import {NotificationService} from "../../../../services/notification.service";
import {NotificationFilter} from "../../../../models/notifications/NotificationFilter";
import {GetListParameters, SortOrder} from "../../../../models/GetListParameters";
import {Notification} from "../../../../models/notifications/Notification";
import {RouterService} from "../../../../services/router.service";
import { AuthService } from "src/app/services/auth.service";
import {NotificationType} from "../../../../models/notifications/NotificationType";

@Component({
  selector: 'notification-bell',
  templateUrl: './notification.bell.component.html',
  styleUrls: ['./notification.bell.component.scss'],
})

export class NotificationBellComponent implements AfterViewInit
{
  Notification = Notification
  NotificationType = NotificationType
  notifications:BaseCollection<Notification> = new BaseCollection<Notification>();
  unreadNotificationsCount:number = 0;

  constructor(
    private notificationService: NotificationService,
    private router: RouterService,
    private authService: AuthService
    ) {}

  ngAfterViewInit(): void {

    this.notificationService.onChanged = _ => {
      if(this.authService.isLoggedIn()) {
        this.updateUnreadNotificationsCount();
        this.fetch();
      }
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
    let filter = new GetListParameters<NotificationFilter>();
    filter.Filter = new NotificationFilter();
    filter.Filter.isRead = false;
    filter.SortOrder = SortOrder.Desc;
    filter.SortBy = "createdAt";

    this.notificationService
      .getNotifications(filter)
      .subscribe(x=> this.notifications = x);
  }

  viewAll = () => this.router.Notifications.List();

  menuOpened(){
    if (this.unreadNotificationsCount > 0)
      this.markAllAsRead();
  }
}
