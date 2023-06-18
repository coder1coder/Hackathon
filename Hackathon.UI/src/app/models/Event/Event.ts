import {ChangeEventStatusMessage} from "./ChangeEventStatusMessage";
import {EventStatus} from "./EventStatus";
import {Team} from "../Team/Team";
import {IUser} from "../User/IUser";
import {EventStage} from "./EventStage";
import {IEventTaskItem} from "./IEventTaskItem";
import {IEventAgreement} from "./IEventAgreement";

export class Event {
  id!: number;
  name!: string;
  description!: string;
  start!: Date;
  maxEventMembers!: number;
  minTeamMembers!: number;
  isCreateTeamsAutomatically!: boolean;
  changeEventStatusMessages!: ChangeEventStatusMessage[];
  status!: EventStatus;
  ownerId?: number;
  owner!: IUser
  teams: Team[] = []
  stages: EventStage[] = []
  award!: string;
  imageId: string;
  imageUrl?: string;
  agreement?: IEventAgreement

  //Задачи, которые ставятся перед участниками мероприятия
  tasks: IEventTaskItem[];

  public static getMemberTeam(event:Event, memberId: number): Team | undefined{
    return event?.teams.find(x=>x.members.findIndex(s=>s.id == memberId) >=0);
  }

  public static hasTasks(event: Event):boolean{
    return event?.tasks?.length > 0
  }

  public static getUsersCount(event:Event): number {
    return event.teams?.reduce((acc, team) => acc + team.members.length, 0);
  }

  public static getMembers(event:Event): IUser[]{
    return event.teams?.flatMap(x=>x.members);
  }
}
