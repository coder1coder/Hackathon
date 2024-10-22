import { IEnvironment } from '../app/models/environment.interface';

export const environment: IEnvironment = {
  production: true,
  api: 'http://hackathon.cleannetcode.ru/api',
  hubs: {
    notification: 'http://hackathon.cleannetcode.ru/api/hubs/notifications',
    chat: 'http://hackathon.cleannetcode.ru/api/hubs/chat',
    friendship: 'http://hackathon.cleannetcode.ru/api/hubs/friendship',
    event: 'http://hackathon.cleannetcode.ru/api/hubs/events',
  },
  reCaptchaKey: '6LdE8XceAAAAACk0qwTv6xMD0Ecml2pb8AQyEEF-',
  captchaEnabled: false,
  googleClientId: '47513176100-9el42bqlf4a92g3midbfnaik6gaagj8q.apps.googleusercontent.com',
};
