import {CreateEvent} from "./CreateEvent";

export interface UpdateEvent extends CreateEvent
{
  id: number;
  userId: number;
}
