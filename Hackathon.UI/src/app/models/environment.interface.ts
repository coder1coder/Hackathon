export interface IEnvironment {
  reCaptchaKey: string;
  production: boolean;
  captchaEnabled: boolean;
  api: string;
  googleClientId: string;
  hubs: IHubs;
}

interface IHubs {
  notification: string;
  friendship: string;
  chat: string;
  event: string;
}
