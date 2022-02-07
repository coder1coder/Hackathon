import {NotificationType} from "./NotificationType";

export class NotificationModel {
  id?:string;
  data:any
  type?: NotificationType
  isRead?: boolean;
  ownerId:number = 0;
  createdAt?:Date;
}
