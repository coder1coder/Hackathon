import {ChangeEventStatusMessage} from "./ChangeEventStatusMessage";
import {EventStatus} from "./EventStatus";
import {Team} from "../Team/Team";
import {IUser} from "../User/IUser";
import {EventStage} from "./EventStage";

export class Event {
  id!: number;
  name!: string;
  description!: string;
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
  stages: EventStage[] = []
  award!: string;
  imageId: string;
  imageUrl?: string;

  public static getUsersCount(event:Event): number {
    return event.teams?.reduce((acc, team) => acc + team.members.length, 0);
  }

  public static getMembers(event:Event): IUser[]{
    return  event.teams?.flatMap(x=>x.members);
  }
}
