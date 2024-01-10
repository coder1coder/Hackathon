import { IEnvironment } from '../app/models/environment.interface';

export const environment: IEnvironment = {
  production: true,
  api: 'http://31.129.103.69/api',
  hubs: {
    notification: 'http://31.129.103.69/api/hubs/notifications',
    chat: 'http://31.129.103.69/api/hubs/chat',
    friendship: 'http://31.129.103.69/api/hubs/friendship',
    event: 'http://31.129.103.69/api/hubs/events',
  },
  reCaptchaKey: '6LdE8XceAAAAACk0qwTv6xMD0Ecml2pb8AQyEEF-',
  captchaEnabled: false,
  googleClientId: '47513176100-9el42bqlf4a92g3midbfnaik6gaagj8q.apps.googleusercontent.com',
};
