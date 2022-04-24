import { ChangeEventStatusMessage } from "./ChangeEventStatusMessage";

export interface CreateEvent
{
  name: string;
  developmentMinutes: number;
  isCreateTeamsAutomatically: boolean;
  maxEventMembers: number;
  memberRegistrationMinutes: number;
  minTeamMembers: number;
  start: Date;
  teamPresentationMinutes: number;
  changeEventStatusMessages: ChangeEventStatusMessage[];
}
