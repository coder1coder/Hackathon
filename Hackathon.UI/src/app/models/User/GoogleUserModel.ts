export class GoogleUserModel
{
  id!: number;
  fullName!: string;
  givenName! : string;
  imageUrl!: string;
  email!: string;
  accessToken!: string;
  expiresAt!: number;
  expiresIn!: number;
  firstIssuedAt!: number;
  TokenId!: string;
  loginHint!: string;
  isLoggedIn!: boolean;
}
