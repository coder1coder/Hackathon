import {Component, Input} from "@angular/core";
import {NotificationService} from "../../../services/notification.service";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  @Input() title!: string;
  @Input() isLoading: boolean = false;

  constructor(private notificationService:NotificationService) {
    notificationService.onPublished = (x) => {
    };
  }
}

