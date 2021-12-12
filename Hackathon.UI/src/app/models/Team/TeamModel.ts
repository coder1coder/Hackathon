import {EventModel} from "../EventModel";

export interface TeamModel {
  id: number,
  name: string,
  eventId: number,
  event: EventModel,
  users: any[], //TODO: add typed
  project: any //TODO: add typed
}
