import { UserRole } from './User/UserRole';

export interface IGetTokenResponse {
  userId: number;
  token: string;
  expires: number;
  role: UserRole;
}
