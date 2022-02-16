export class GoogleUserModel {
  id!: string;
  fullName!: string;
  givenName! : string;
  imageUrl!: string;
  email!: string;
  accessToken!: string;
  expiresAt!: number;
  expiresIn!: number;
  firstIssuedAt!: number;
  idToken!: string;
  loginHint!: string;
  isLoggedIn!: boolean;
}
