import { EventModel } from "../Event/EventModel";
import {TeamModel} from "./TeamModel";

export interface TeamEventModel{
  event: EventModel
  eventId: number
  project: any
  projectId: number
  team: TeamModel
  teamId: number
}
