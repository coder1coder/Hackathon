import {HttpClient, HttpHeaders} from '@angular/common/http';
import {EventEmitter, Injectable} from '@angular/core';
import {Observable} from 'rxjs/internal/Observable';
import {environment} from "../../environments/environment";
import {map} from "rxjs/operators";
import {ICreateUser} from '../models/User/CreateUser';
import {IGetTokenResponse} from "../models/IGetTokenResponse";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {AuthConstants} from "./auth.constants";
import {GoogleSigninService} from './google-signin.service';
import {GoogleUser} from '../models/User/GoogleUser';
import {IUser} from "../models/User/IUser";

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  public authChange: EventEmitter<boolean> = new EventEmitter();

  private api: string = environment.api;
  private storage: Storage = sessionStorage;

  constructor(
    private http: HttpClient,
    private googleSigninService: GoogleSigninService
    ) {
  }

  public isLoggedIn(): boolean {
    let tokenInfo = this.getTokenInfo();
    if (tokenInfo == undefined)
      return false;

    return tokenInfo.expires >= Date.now();
  }

  public login(login: string, password: string) : Observable<IGetTokenResponse> {
    return this.http.post<IGetTokenResponse>(this.api+"/Auth/SignIn", {username: login, password: password})
      .pipe(map(r=>{
        this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(r));
        this.authChange.emit(true);
        return r;
      }));
  }

  public register(createUser: ICreateUser): Observable<IBaseCreateResponse> {
    return this.http.post<IBaseCreateResponse>(this.api+"/User", createUser);
  }

  public logout(): void {
    this.storage.removeItem(AuthConstants.STORAGE_AUTH_KEY);
    this.storage.removeItem(AuthConstants.STORAGE_USER_KEY);
    this.authChange.emit(false);
  }

  public getUserId(): number | undefined {
    let tokenInfo = this.getTokenInfo();
    return tokenInfo?.userId;
  }

  public getCurrentUser(): Observable<IUser> | null {
    if (!this.isLoggedIn())
      return null;

    const tokenInfo = this.getTokenInfo();
    if (tokenInfo == undefined)
      return null;

    return this.http.get<IUser>(this.api+'/User/'+tokenInfo.userId, {
      headers: new HttpHeaders()
        .append('Authorization', 'Bearer '+tokenInfo.token)
      });
  }

  public loginByGoogle(googleUserModel: GoogleUser) : Observable<IGetTokenResponse> {
    return this.http.post<IGetTokenResponse>(this.api+"/Auth/SignInByGoogle", {
      id: googleUserModel.id,
      fullName: googleUserModel.fullName,
      givenName: googleUserModel.givenName ,
      imageUrl: googleUserModel.imageUrl,
      email: googleUserModel.email,
      accessToken: googleUserModel.accessToken,
      expiresAt: googleUserModel.expiresAt,
      expiresIn: googleUserModel.expiresIn,
      firstIssuedAt: googleUserModel.firstIssuedAt,
      TokenId: googleUserModel.TokenId,
      loginHint: googleUserModel.loginHint,
      isLoggedIn: googleUserModel.isLoggedIn
    }).pipe(map(r=>{
        this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(r));
      this.authChange.emit(true);
        return r;
      }));
  }

  public signInByGoogle(): Promise<void | gapi.auth2.GoogleUser> {
    return this.googleSigninService.signIn();
  }

  public isGoogleClientEnabled(): Observable<boolean> {
    return this.googleSigninService.getGoogleServiceEnabled$;
  }

  private getTokenInfo() : IGetTokenResponse | undefined {
    let authInfo = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);
    if (authInfo == undefined)
      return undefined;

    return JSON.parse(authInfo);
  }
}
