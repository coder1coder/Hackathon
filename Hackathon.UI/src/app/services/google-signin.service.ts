import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment.prod';
import {Observable, ReplaySubject} from 'rxjs';
import {ProblemDetails} from '../models/ProblemDetails';

@Injectable({
  providedIn: 'root'
})
export class GoogleSigninService {

  private auth2!: gapi.auth2.GoogleAuth;
  private subject = new ReplaySubject<gapi.auth2.GoogleUser>(1);

  constructor() {
    gapi.load('auth2', () => {
      this.auth2 = gapi.auth2.init({
        client_id: environment.googleClientId
      })
    })
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
          let details: ProblemDetails = <ProblemDetails>error.error;
          errorMessage = details.detail;
        }
     });
  }

  public signOut() : any {
    return this.auth2.signOut();
  }

  Observable() : Observable<gapi.auth2.GoogleUser> {
     return this.subject.asObservable();
  }
}
