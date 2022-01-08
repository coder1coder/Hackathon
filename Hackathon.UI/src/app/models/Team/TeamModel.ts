import {EventModel} from "../EventModel";
import {UserModel} from "../User/UserModel";
import {CreateTeamModel} from "./CreateTeamModel";

export interface TeamModel extends CreateTeamModel {
  id: number,
  event: EventModel,
  users: UserModel[],
  project: any //TODO: add typed
}
