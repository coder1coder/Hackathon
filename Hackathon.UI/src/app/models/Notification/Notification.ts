import { NotificationType } from "./NotificationType";

export class Notification {
  id?: string;
  data: string;
  type?: NotificationType;
  isRead?: boolean;
  ownerId: number = 0;
  createdAt?: Date;

  public static getParsedData<T>(data: string): T | null {
    return JSON.parse(data);
  }

  public static getTypeName(type?:NotificationType): string
  {
    switch (type){
      case NotificationType.Information:
        return 'Системное уведомление';
      case NotificationType.TeamInvite:
        return 'Приглашение в команду';
      case NotificationType.EventInvite:
        return 'Приглашение в событие';
      case NotificationType.TeamJoinRequestDecision:
        return 'Ответ на запрос вступления в команду';
      default:
        return ``;
    }
  }

  public static getTypeIcon(type?:NotificationType): string {
    switch (type) {
      case NotificationType.Information:
        return 'info_outline';
      case NotificationType.TeamInvite:
        return 'group_add';
      case NotificationType.EventInvite:
        return 'calendar_add_on';
      default:
        return 'info_outline';
    }
  }
}
