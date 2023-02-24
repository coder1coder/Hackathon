import { ChangeEventStatusMessage } from "./ChangeEventStatusMessage";
import {EventStage} from "./EventStage";

export interface ICreateEvent
{
  name: string;
  description:string;
  developmentMinutes: number;
  isCreateTeamsAutomatically: boolean;
  maxEventMembers: number;
  memberRegistrationMinutes: number;
  minTeamMembers: number;
  start: Date;
  teamPresentationMinutes: number;
  changeEventStatusMessages: ChangeEventStatusMessage[];
  stages: EventStage[];
  award:string;
  imageId:string;
}
