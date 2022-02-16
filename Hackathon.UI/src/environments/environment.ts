// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  api: "http://localhost:5000/api",
  messageHub: "http://localhost:5000/notifications",
  reCaptchaKey: "6Lew_2YeAAAAAFuRFhML7FxNeiD4wZnrX6x9GF_a",
  googleClientId: "47513176100-ti42tnck4g0eo8uqenebo74gb37so4l0.apps.googleusercontent.com"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
