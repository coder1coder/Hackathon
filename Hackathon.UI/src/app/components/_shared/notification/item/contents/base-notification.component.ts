import {Component, EventEmitter, Inject, Input, Output} from "@angular/core";
import {NotificationViewModel} from "../NotificationViewModel";
import {Notification} from "src/app/models/notifications/Notification";
import {NotificationType} from "src/app/models/notifications/NotificationType";

@Component({
  selector: 'base-notification-component',
  template: ''
})

export abstract class BaseNotificationComponent
{
  Notification = Notification;

  @Input()
  set notification(value: Notification) {
    if (this._viewModel)
      this._viewModel.notification = value;
  }

  get notification(): Notification {
    return this._viewModel?.notification;
  }

  private _viewModel: NotificationViewModel = new NotificationViewModel();

  get viewModel(): NotificationViewModel{
    return this._viewModel;
  }
  set viewModel(viewModel: NotificationViewModel){
    this._viewModel = viewModel;
  }

  @Output() onRemove = new EventEmitter<string>();

  protected constructor(
    @Inject(String) icon: string,
    @Inject(String) title: string) {

    this.viewModel = {
      icon: icon,
      title: title,
      notification: this.notification
    }
  }

  protected resolveNotificationTitle(): string
  {
    switch (this.notification.type){
      case NotificationType.TeamInvite: return 'Приглашение в команду';
      case NotificationType.EventInvite: return 'Приглашение на мероприятие';
      case NotificationType.TeamAcceptance: return `Запрос на вступление в команду принят`;
      case NotificationType.JoinTeamRejection: return `Запрос на вступление в команду отклонен`;

      case NotificationType.FriendshipAccepted: return `Запрос дружбы принят`;
      case NotificationType.EventWillStartSoon: return `Мероприятие скоро начнется`;
      case NotificationType.NewTeamChatMessage: return `Новое сообщение в чате команды`;
      case NotificationType.NewEventChatMessage: return `Новое сообщение в чате мероприятия`;
      default: return '';
    }
  }
}



