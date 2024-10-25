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
  chats: IChatHubs;
  event: string;
}

interface IChatHubs {
  events: string;
  teams: string;
}
