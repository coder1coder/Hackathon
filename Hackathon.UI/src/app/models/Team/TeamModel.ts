import {ITeamEventModel} from "./ITeamEventModel";
import {IUserModel} from "../User/IUserModel";

export class TeamModel {
  id!: number;
  name?: string;
  teamEvents:ITeamEventModel[] = [];
  users: IUserModel[] = [];
  owner?: IUserModel;
  project: any //TODO: add typed
}
