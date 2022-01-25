import {EventStatus} from "./EventStatus";
import {UserModel} from "./User/UserModel";
import {TeamModel} from "./Team/TeamModel";
import { ChangeEventStatusMessage } from "./Event/ChangeEventStatusMessage";
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
  ChangeEventStatusMessages!: ChangeEventStatusMessage[];
  status!: EventStatus;
  userId!: number;
  user!: UserModel;
  teamEvents: TeamEventModel[] = []
}
