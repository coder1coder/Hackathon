import { Component, Input } from "@angular/core";
import { Notification } from "src/app/models/Notification/Notification";
import { NotificationService } from "src/app/services/notification.service";

@Component({
    selector: `notification-item`,
    templateUrl: `notification-item.component.html`,
    styleUrls: [`notification-item.component.scss`]
})
export class NotificationItemComponent
{
    Notification = Notification;

    @Input() notification: Notification | undefined; 

    constructor(
        private notificationService: NotificationService
        ) {
        }

    public remove(event:MouseEvent, ids:string[]) {
        this.notificationService.remove(ids).subscribe(_=>
        {
            // this.items = this.items.filter(x => x.id !== undefined && !ids.includes(x.id));
        });
    }
}