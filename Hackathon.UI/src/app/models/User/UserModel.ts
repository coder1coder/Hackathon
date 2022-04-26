import {UserRole} from "./UserRole";

export class UserModel{
  id?: number;
  userName?: string;
  email?: string;
  fullName?: string;
  role:UserRole = UserRole.Default;
  profileImageId?: string
}
