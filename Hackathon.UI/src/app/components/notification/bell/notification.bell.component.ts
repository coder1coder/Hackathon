import {AfterViewInit, Component, ViewChild} from "@angular/core";
import {Router} from "@angular/router";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {NotificationModel} from "../../../models/Notification/NotificationModel";
import {NotificationService} from "../../../services/notification.service";
import {NotificationFilterModel} from "../../../models/Notification/NotificationFilterModel";
import {GetFilterModel, SortOrder} from "../../../models/GetFilterModel";
import {Notification} from "../../../models/Notification/Notification";

@Component({
  selector: 'notification-bell',
  templateUrl: './notification.bell.component.html',
  styleUrls: ['./notification.bell.component.scss'],
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

  remove(event:MouseEvent, ids:string[]){
      event.stopPropagation();
      this.notificationService.remove(ids).subscribe(_=>{
        this.notifications.items = this.notifications.items.filter(x=> x.id !== undefined && !ids.includes(x.id));
        this.notifications.totalCount = this.notifications.totalCount - ids.length;
      });
  }

  removeAll(event:MouseEvent){
    let ids = this.notifications.items.map(x=>x.id!);
    this.remove(event, ids);
  }
}
