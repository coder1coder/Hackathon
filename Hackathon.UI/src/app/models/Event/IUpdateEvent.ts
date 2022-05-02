import {ICreateEvent} from "./ICreateEvent";

export interface IUpdateEvent extends ICreateEvent
{
  id: number;
  userId: number;
}
