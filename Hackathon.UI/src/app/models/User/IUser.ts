import {UserRole} from "./UserRole";

export interface IUser {
  id?: number
  userName?: string
  email?: string
  fullName?: string
  role: UserRole
  profileImageId?: string
}
