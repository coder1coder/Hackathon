import { EventModel } from "../Event/EventModel";
import {TeamModel} from "./TeamModel";

export interface ITeamEventModel {
  event: EventModel
  eventId: number
  project: any
  projectId: number
  team: TeamModel
  teamId: number
}
