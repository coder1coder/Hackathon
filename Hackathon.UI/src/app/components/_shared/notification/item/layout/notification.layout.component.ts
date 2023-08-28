import {Component, EventEmitter, Input, Output} from "@angular/core";
import {NotificationViewModel} from "../NotificationViewModel";

@Component({
  selector: 'notification-layout',
  templateUrl: './notification.layout.component.html',
  styleUrls: ['./notification.layout.component.scss']
})

export class NotificationLayoutComponent
{
  @Input() viewModel: NotificationViewModel;
  @Output() onRemove = new EventEmitter<string>();
}
