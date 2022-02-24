import {UserModel} from "../User/UserModel";
import {EventModel} from "../Event/EventModel";
import {TeamEventModel} from "./TeamEventModel";

export class TeamModel {
  id!: number;
  name?: string;
  teamEvents:TeamEventModel[] = [];
  users: UserModel[] = [];
  owner?: UserModel;
  project: any //TODO: add typed
}
