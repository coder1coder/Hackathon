import {NotificationType} from "./NotificationType";

export class Notification
{
  id?:string;
  data:any
  type?: NotificationType
  isRead?: boolean;
  ownerId:number = 0;
  createdAt?:Date;

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
