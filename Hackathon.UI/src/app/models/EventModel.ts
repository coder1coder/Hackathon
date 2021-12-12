import {EventStatus} from "./EventStatus";

export interface EventModel {
  id: number;
  name: string;
  start: Date;
  memberRegistrationMinutes: number;
  developmentMinutes: number;
  teamPresentationMinutes: number;
  maxEventMembers: number;
  minTeamMembers: number;
  isCreateTeamsAutomatically: boolean;
  changeEventStatusMessages: any[];
  status: EventStatus;
  teams: any[]
}
