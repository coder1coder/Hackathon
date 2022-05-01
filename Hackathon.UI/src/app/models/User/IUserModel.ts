import {UserRole} from "./UserRole";

export interface IUserModel{
  id?: number
  userName?: string
  email?: string
  fullName?: string
  role: UserRole
  profileImageId?: string
}
