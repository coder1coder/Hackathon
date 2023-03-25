export interface ITeamJoinRequest
{
  id:number;
  teamId:number;
  teamName:string;
  userId:number;
  createdAt:Date;
  modifyAt?:Date;
  status: TeamJoinRequestStatus;
  comment:string;
}

export enum TeamJoinRequestStatus
{
  /// <summary>
  /// Запрос отправлен
  /// </summary>
  Sent = 0,

  /// <summary>
  /// Запрос принят
  /// </summary>
  Accepted = 1,

  /// <summary>
  /// Запрос отклонен
  /// </summary>
  Refused = 2,

  /// <summary>
  /// Запрос отменен автором
  /// </summary>
  Cancelled = 3
}

export class TeamJoinRequestStatusTranslator {
  public static GetName = (e:TeamJoinRequestStatus) : string => TeamJoinRequestStatus[e].toLowerCase();
  public static Translate = (e: TeamJoinRequestStatus) : string => {
    switch (e) {
      case TeamJoinRequestStatus.Sent: return 'Запрос отправлен'
      case TeamJoinRequestStatus.Accepted: return 'Запрос принят'
      case TeamJoinRequestStatus.Cancelled: return 'Отменено пользователем'

      default: return TeamJoinRequestStatus[e];
    }
  }
}
