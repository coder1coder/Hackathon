import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';
import { ICreateUser } from '../models/User/CreateUser';
import { IGetTokenResponse } from '../models/IGetTokenResponse';
import { IBaseCreateResponse } from '../models/IBaseCreateResponse';
import { AuthConstants } from './auth.constants';
import { GoogleSignInService } from './google-sign-in.service';
import { GoogleUser } from '../models/User/GoogleUser';
import { IUser } from '../models/User/IUser';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public authChange: EventEmitter<boolean> = new EventEmitter();

  private api: string = environment.api;
  private storage: Storage = sessionStorage;

  constructor(private http: HttpClient, private googleSignInService: GoogleSignInService) {}

  public isLoggedIn(): boolean {
    const tokenInfo: IGetTokenResponse = this.getTokenInfo();
    if (!tokenInfo) return false;

    return tokenInfo.expires >= Date.now();
  }

  public login(login: string, password: string): Observable<IGetTokenResponse> {
    return this.http
      .post<IGetTokenResponse>(this.api + '/Auth/SignIn', { username: login, password: password })
      .pipe(
        map((r) => {
          this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(r));
          this.authChange.emit(true);
          return r;
        }),
      );
  }

  public register(createUser: ICreateUser): Observable<IBaseCreateResponse> {
    return this.http.post<IBaseCreateResponse>(this.api + '/User', createUser);
  }

  public logout(): void {
    this.storage.removeItem(AuthConstants.STORAGE_AUTH_KEY);
    this.storage.removeItem(AuthConstants.STORAGE_USER_KEY);
    this.authChange.emit(false);
  }

  public getUserId(): number {
    const tokenInfo: IGetTokenResponse = this.getTokenInfo();
    return tokenInfo?.userId ?? null;
  }

  public getCurrentUser(): Observable<IUser> {
    const tokenInfo: IGetTokenResponse = this.getTokenInfo();
    if (!this.isLoggedIn() || !tokenInfo) {
      return of(null);
    }

    return this.http.get<IUser>(this.api + '/User/' + tokenInfo.userId, {
      headers: new HttpHeaders().append('Authorization', 'Bearer ' + tokenInfo.token),
    });
  }

  public loginByGoogle(googleUserModel: GoogleUser): Observable<IGetTokenResponse> {
    return this.http
      .post<IGetTokenResponse>(this.api + '/Auth/SignInByGoogle', {
        id: googleUserModel.id,
        fullName: googleUserModel.fullName,
        givenName: googleUserModel.givenName,
        imageUrl: googleUserModel.imageUrl,
        email: googleUserModel.email,
        accessToken: googleUserModel.accessToken,
        expiresAt: googleUserModel.expiresAt,
        expiresIn: googleUserModel.expiresIn,
        firstIssuedAt: googleUserModel.firstIssuedAt,
        TokenId: googleUserModel.TokenId,
        loginHint: googleUserModel.loginHint,
        isLoggedIn: googleUserModel.isLoggedIn,
      })
      .pipe(
        map((res) => {
          this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(res));
          this.authChange.emit(true);
          return res;
        }),
      );
  }

  public signInByGoogle(): Promise<any> {
    return this.googleSignInService.signIn();
  }

  public isGoogleClientEnabled(): Observable<boolean> {
    return this.googleSignInService.getGoogleServiceEnabled$;
  }

  private getTokenInfo(): IGetTokenResponse {
    const authInfo: string = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);
    if (!authInfo) return null;
    return JSON.parse(authInfo);
  }
}
