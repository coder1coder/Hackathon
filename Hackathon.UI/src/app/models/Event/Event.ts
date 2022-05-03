import {ChangeEventStatusMessage} from "./ChangeEventStatusMessage";
import {EventStatus} from "../EventStatus";
import {Team} from "../Team/Team";
import {IUser} from "../User/IUser";

export class Event {
  id!: number;
  name!: string;
  start!: Date;
  memberRegistrationMinutes!: number;
  developmentMinutes!: number;
  teamPresentationMinutes!: number;
  maxEventMembers!: number;
  minTeamMembers!: number;
  isCreateTeamsAutomatically!: boolean;
  changeEventStatusMessages!: ChangeEventStatusMessage[];
  status!: EventStatus;
  ownerId?: number;
  owner!: IUser
  teams: Team[] = []
  stages: any[] = []

  public static getUsersCount(event:Event):number{
    let i = 0;
    Event.getEventTeams(event).forEach(x=> i += x.members?.length ?? 0);
    return i;
  }

  public static getEventTeams(event:Event):Team[]{
    return event.teams ?? [];
  }
}
