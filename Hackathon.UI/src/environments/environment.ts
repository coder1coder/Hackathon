// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  api: "http://localhost:5000/api",
  hubs:{
    notification: "http://localhost:5000/hubs/notifications",
    chat: "http://localhost:5000/hubs/chat",
  },
  reCaptchaKey: "6Lew_2YeAAAAAFuRFhML7FxNeiD4wZnrX6x9GF_a",
  captchaEnabled: false,
  googleClientId: "47513176100-9el42bqlf4a92g3midbfnaik6gaagj8q.apps.googleusercontent.com"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
