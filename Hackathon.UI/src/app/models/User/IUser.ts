import { UserRole } from "./UserRole";
import { IUserEmail } from "./IUserEmail";
import { SafeUrl } from "@angular/platform-browser";

export interface IUser extends IAdditionalInfo {
  id?: number;
  userName?: string;
  email?: IUserEmail;
  fullName?: string;
  role: UserRole;
  profileImageId?: string;
}

export interface IUpdateUser {
  id: number;
  email: string;
  fullName: string;
}

interface IAdditionalInfo {
  /** заполняется с фронтэнда */
  shortUserName?: string;
  image?: SafeUrl;
  bgColor?: string;
}
