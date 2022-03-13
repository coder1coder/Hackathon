import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs/internal/Observable';
import {environment} from "../../environments/environment";
import {map} from "rxjs/operators";
import {CreateUser} from '../models/CreateUser';
import {GetTokenResponse} from "../models/GetTokenResponse";
import {BaseCreateResponse} from "../models/BaseCreateResponse";
import {UserModel} from "../models/User/UserModel";
import {AuthConstants} from "./auth.constants";
import {GoogleSigninService} from './google-signin.service';
import {GoogleUserModel} from '../models/User/GoogleUserModel';

@Injectable({
  providedIn: 'root'
})

export class AuthService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient,
    private googleSigninService: GoogleSigninService) {
  }

  isLoggedIn() {
    let tokenInfo = this.#getTokenInfo();
    if (tokenInfo == undefined)
      return false;

    return tokenInfo.expires >= Date.now();
  }

  #getTokenInfo(){
    let authInfo = this.storage.getItem(AuthConstants.STORAGE_AUTH_KEY);
    if (authInfo == undefined)
      return undefined;

    let auth:GetTokenResponse = JSON.parse(authInfo);
    return auth;
  }

  login(login: string, password: string): Observable<GetTokenResponse> {
    return this.http.post<GetTokenResponse>(this.api+"/Auth/SignIn", {username: login, password: password})
      .pipe(map(r=>{
        this.storage.setItem(AuthConstants.STORAGE_AUTH_KEY, JSON.stringify(r));
        return r;
      }));
  }

  register(createUser:CreateUser): Observable<BaseCreateResponse>{
    return this.http.post<BaseCreateResponse>(this.api+"/User", {
      userName: createUser.userName,
      password: createUser.password,
      FullName: createUser.fullname,
      email: createUser.email
    })
  }

  logout(){
    this.storage.removeItem(AuthConstants.STORAGE_AUTH_KEY);
    this.storage.removeItem(AuthConstants.STORAGE_USER_KEY);
  }

  getUserId():number | undefined{
    let tokenInfo = this.#getTokenInfo();
    return tokenInfo?.userId;
  }

  getCurrentUser(): Observable<UserModel> | null{
    if (!this.isLoggedIn())
      return null;

    let tokenInfo = this.#getTokenInfo();
    if (tokenInfo == undefined)
      return null;

    return this.http.get<UserModel>(this.api+'/User/'+tokenInfo.userId, {
      headers: new HttpHeaders()
        .append('Authorization', 'Bearer '+tokenInfo.token)
      });
  }

  loginByGoogle(googleUserModel: GoogleUserModel): Observable<GetTokenResponse> {
    return this.http.post<GetTokenResponse>(this.api+"/Auth/SignInByGoogle", {
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
      return r;
    }));
}

  signInByGoogle() {
    return this.googleSigninService.signIn();
  }

  signOutByGoogle() {
    this.storage.removeItem(AuthConstants.STORAGE_AUTH_KEY);
    this.storage.removeItem(AuthConstants.STORAGE_USER_KEY);
    return this.googleSigninService.signOut();
  }
}
