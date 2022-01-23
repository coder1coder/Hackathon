import {EventStatus} from "./EventStatus";
import {UserModel} from "./User/UserModel";
import {TeamEventModel} from "./Team/TeamEventModel";

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
  changeEventStatusMessages!: any[];
  status!: EventStatus;
  userId!: number;
  user!: UserModel;
  teamEvents: TeamEventModel[] = []
}
