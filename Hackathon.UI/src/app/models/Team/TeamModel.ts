import {TeamEventModel} from "./TeamEventModel";
import {IUserModel} from "../User/IUserModel";

export class TeamModel {
  id!: number;
  name?: string;
  teamEvents:TeamEventModel[] = [];
  users: IUserModel[] = [];
  owner?: IUserModel;
  project: any //TODO: add typed
}
