import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';
import { BehaviorSubject } from 'rxjs';
import { SnackService } from './snack.service';
import { ActionsEnum } from '../common/emuns/actions.enum';

@Injectable({
  providedIn: 'root',
})
export class GoogleSignInService {
  private auth2: any;
  private googleServiceEnabled$ = new BehaviorSubject<boolean>(true);
  public getGoogleServiceEnabled$ = this.googleServiceEnabled$.asObservable();

  constructor(private snackService: SnackService) {
    try {
      // eslint-disable-next-line no-undef
      gapi.load('auth2', () => {
        // eslint-disable-next-line no-undef
        this.auth2 = gapi.auth2.init({
          client_id: environment.googleClientId,
        });
      });
    } catch (e) {
      this.googleServiceEnabled$.next(false);
      this.snackService.open('Google сервис не доступен в оффлайн режиме', ActionsEnum.OK, 4000);
    }
  }

  public signIn(): Promise<any> {
    return this.auth2.signIn({
      scope: 'https://www.googleapis.com/auth/gmail.readonly',
    });
  }
}
