import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment.prod';
import {BehaviorSubject, Observable, ReplaySubject} from 'rxjs';
import {IProblemDetails} from '../models/IProblemDetails';
import {SnackService} from "./snack.service";
import {Actions} from "../common/Actions";

@Injectable({
  providedIn: 'root'
})
export class GoogleSigninService {

  private googleServiceEnabled$ = new BehaviorSubject<boolean>(true);
  public getGoogleServiceEnabled$ = this.googleServiceEnabled$.asObservable();

  private auth2!: gapi.auth2.GoogleAuth;
  private subject = new ReplaySubject<gapi.auth2.GoogleUser>(1);

  constructor(
    private snackService: SnackService,
  ) {
    try {
      gapi.load('auth2', () => {
        this.auth2 = gapi.auth2.init({
          client_id: environment.googleClientId
        })
      })
    } catch (e) {
      this.googleServiceEnabled$.next(false);
      this.snackService.open('Google сервис не доступен в оффлайн режиме', Actions.OK, 4000);
    }
  }

  public async signIn() : Promise<void | gapi.auth2.GoogleUser>{
     return this.auth2.signIn({
      scope: 'https://www.googleapis.com/auth/gmail.readonly'
     }).then(user => {
        this.subject.next(user);
        return user;
     }).catch((error) => {
        let errorMessage = "Неизвестная ошибка при авторизация через Google сервис";
        if (error.error.detail !== undefined) {
          let details: IProblemDetails = <IProblemDetails>error.error;
          this.snackService.open(details.detail ?? errorMessage);
        }
     });
  }

  Observable(): Observable<gapi.auth2.GoogleUser> {
     return this.subject.asObservable();
  }
}
