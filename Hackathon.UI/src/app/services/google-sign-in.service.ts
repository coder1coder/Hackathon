import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { BehaviorSubject } from 'rxjs';
import { SnackService } from "./snack.service";
import { Actions } from "../common/Actions";

@Injectable({
  providedIn: 'root'
})
export class GoogleSignInService {

  private auth2: gapi.auth2.GoogleAuth;
  private googleServiceEnabled$ = new BehaviorSubject<boolean>(true);
  public getGoogleServiceEnabled$ = this.googleServiceEnabled$.asObservable();

  constructor(private snackService: SnackService) {
    try {
      gapi.load('auth2', () => {
        this.auth2 = gapi.auth2.init({
          client_id: environment.googleClientId
        });
      })
    } catch (e) {
      this.googleServiceEnabled$.next(false);
      this.snackService.open('Google сервис не доступен в оффлайн режиме', Actions.OK, 4000);
    }
  }

  public signIn(): Promise<gapi.auth2.GoogleUser> {
    return this.auth2.signIn({
      scope: 'https://www.googleapis.com/auth/gmail.readonly',
     });
  }
}
