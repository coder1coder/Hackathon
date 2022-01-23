import {EventModel} from "../EventModel";
import {UserModel} from "../User/UserModel";
import {CreateTeamModel} from "./CreateTeamModel";

export interface TeamModel {
  id: number,
  name: string,
  events: EventModel[],
  users: UserModel[],
  owner: UserModel,
  project: any //TODO: add typed
}
