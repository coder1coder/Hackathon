import {IUser} from "../User/IUser";

export class Team implements ITeam {
  id!: number;
  name?: string;
  members: IUser[] = [];
  owner?: IUser;
  project: any //TODO: add typed
  type: TeamType
}

export interface ITeam {
  id: number;
  name?: string;
  members: IUser[];
  owner?: IUser;
  project: any //TODO: add typed
  type: TeamType;
}

export enum TeamType
{
  Private = 0,
  Public = 1
}
