import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from 'src/environments/environment.prod';
import {MatSnackBar} from "@angular/material/snack-bar";
import {Observable, ReplaySubject} from 'rxjs';
import {ProblemDetails} from '../models/ProblemDetails';
import {GoogleUserModel} from '../models/User/GoogleUserModel';
import {map} from "rxjs/operators";
import { AuthConstants } from './auth.constants';
import { GetTokenResponse } from '../models/GetTokenResponse';

@Injectable({
  providedIn: 'root'
})
export class GoogleSigninService {

  auth2!: gapi.auth2.GoogleAuth;
  subject = new ReplaySubject<gapi.auth2.GoogleUser>(1);
  storage = sessionStorage;
  api = environment.api;

  constructor(private snackBar: MatSnackBar, private http: HttpClient) {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: environment.googleClientId
      })
    })
   }

   signIn() {
     this.auth2.signIn({
      scope: 'https://www.googleapis.com/auth/gmail.readonly'
     }).then(user => {
        this.subject.next(user);
     }).catch((error) => {
        let errorMessage = "Неизвестная ошибка при авторизация через Google сервис";

        if (error.error.detail !== undefined) {
          let details: ProblemDetails = <ProblemDetails>error.error;
          errorMessage = details.detail;
        }
        this.snackBar.open(errorMessage, "ok", { duration: 5 * 1000 });
     });
   }

   signOut() {
     this.auth2.signOut()
     .then(() => {
        this.snackBar.open("Вы успешно вышли из системы!", "ok", { duration: 5 * 500 });
     });
   }

   login(googleUserModel: GoogleUserModel): Observable<GetTokenResponse> {
     console.log('login googleUserModel: ', googleUserModel);
    return this.http.post<GetTokenResponse>(this.api+"/Auth/SignInByGoogle", googleUserModel)
      .pipe(map(r=>{
        this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(r));
        return r;
      }));
  }

   Observable() : Observable<gapi.auth2.GoogleUser> {
     return this.subject.asObservable();
   }

   #getTokenInfo(){
    let authInfo = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);

    if (authInfo == undefined)
      return undefined;

    let auth:GetTokenResponse = JSON.parse(authInfo);

    return auth;
  }

  isLoggedIn() {
    let tokenInfo = this.#getTokenInfo();

    if (tokenInfo == undefined)
      return false;

    return tokenInfo.expires >= Date.now();
  }
}
