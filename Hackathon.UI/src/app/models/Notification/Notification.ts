import {NotificationType} from "./NotificationType";

export class Notification
{
  public static getParsedData(data:any): any {
    return JSON.parse(data);
  }

  public static getTypeName(type:NotificationType){
    switch (type){
      case NotificationType.Information:
        return 'Информация';
      case NotificationType.TeamInvite:
        return 'Приглашение в команду';
      case NotificationType.EventInvite:
        return 'Приглашение в событие';
    }
  }
}
