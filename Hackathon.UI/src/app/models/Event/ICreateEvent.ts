import { ChangeEventStatusMessage } from "./ChangeEventStatusMessage";
import { EventStage } from "./EventStage";
import { IEventTaskItem } from "./IEventTaskItem";
import { IEventAgreement } from "./IEventAgreement";

export interface ICreateEvent {
  /** наименование */
  name: string;
  /** описание */
  description: string;
  isCreateTeamsAutomatically: boolean;
  maxEventMembers: number;
  minTeamMembers: number;
  /** дата и время начала */
  start: Date;
  changeEventStatusMessages: ChangeEventStatusMessage[];
  /** Этапы события */
  stages: EventStage[];
  /** Награда, призовой фонд */
  award: string;
  /** Идентификатор изображения */
  imageId: string;
  /** Соглашение об участии */
  agreement?: IEventAgreement;
  /** задачи, которые ставятся перед участниками мероприятия */
  tasks: IEventTaskItem[];
}
