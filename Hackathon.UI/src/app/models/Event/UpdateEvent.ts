import {CreateEvent} from "./CreateEvent";

export class UpdateEvent extends CreateEvent
{
  id!:number
  userId!: number
}
