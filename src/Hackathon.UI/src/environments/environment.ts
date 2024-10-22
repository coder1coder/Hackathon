import { IEnvironment } from '../app/models/environment.interface';

export const environment: IEnvironment = {
  production: false,
  api: 'http://localhost:5000/api',
  hubs: {
    notification: 'http://localhost:5000/hubs/notifications',
    chat: 'http://localhost:5000/hubs/chat',
    friendship: 'http://localhost:5000/hubs/friendship',
    event: 'http://localhost:5000/hubs/events',
  },
  reCaptchaKey: '6Lew_2YeAAAAAFuRFhML7FxNeiD4wZnrX6x9GF_a',
  captchaEnabled: false,
  googleClientId: '47513176100-9el42bqlf4a92g3midbfnaik6gaagj8q.apps.googleusercontent.com',
};
