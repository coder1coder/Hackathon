import {UserRole} from "./User/UserRole";

export interface GetTokenResponse
{
  userId: number;
  token: string;
  expires: number;
  role: UserRole;
}
