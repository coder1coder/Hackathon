import { ChangeEventStatusMessage } from "./ChangeEventStatusMessage";
import {EventStage} from "./EventStage";

export interface ICreateEvent
{
  //наименование
  name: string;

  //описание
  description:string;
  developmentMinutes: number;
  isCreateTeamsAutomatically: boolean;
  maxEventMembers: number;
  memberRegistrationMinutes: number;
  minTeamMembers: number;

  //дата и время начала
  start: Date;
  teamPresentationMinutes: number;
  changeEventStatusMessages: ChangeEventStatusMessage[];

  //Этапы события
  stages: EventStage[];

  //Награда, призовой фонд
  award:string;

  //Идентификатор изображения
  imageId:string;

  //Правила участия
  rules: string;
}
