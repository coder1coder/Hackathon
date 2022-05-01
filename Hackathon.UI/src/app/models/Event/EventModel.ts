import {ChangeEventStatusMessage} from "./ChangeEventStatusMessage";
import {TeamEventModel} from "../Team/TeamEventModel";
import {EventStatus} from "../EventStatus";
import {TeamModel} from "../Team/TeamModel";
import {IUserModel} from "../User/IUserModel";

export class EventModel {
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
  userId?: number;
  user!: IUserModel
  teamEvents: TeamEventModel[] = []
  stages: any[] = []

  public static getUsersCount(event:EventModel):number{
    let i = 0;
    EventModel.getEventTeams(event).forEach(x=> i += x.users?.length ?? 0);
    return i;
  }

  public static getEventTeams(event:EventModel):TeamModel[]{
    return event.teamEvents?.map(x=>x.team) ?? [];
  }
}
