import {UserRole} from "./UserRole";
import {IUserEmail} from "./IUserEmail";

export interface IUser {
  id?: number
  userName?: string
  email?: IUserEmail
  fullName?: string
  role: UserRole
  profileImageId?: string
}

export interface IUpdateUser {
  id: number;
  email: string;
  fullName: string;
}
