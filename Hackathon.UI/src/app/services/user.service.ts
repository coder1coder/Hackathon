import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { BaseCollection } from "../models/BaseCollection";
import { GetListParameters } from "../models/GetListParameters";
import { UserFilter } from "../models/User/UserFilter";
import { IUser } from "../models/User/IUser";

@Injectable({
  providedIn: 'root'
})

export class UserService {

  private api: string = environment.api;

  constructor(private http: HttpClient) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  public getList(getFilterModel:GetListParameters<UserFilter>):Observable<BaseCollection<IUser>>{
    return this.http.post<BaseCollection<IUser>>(this.api+'/User/list', getFilterModel);
  }

  public getById(userId:number): Observable<IUser> {
    return this.http.get<IUser>(this.api+'/User/'+ userId);
  }

  public setImage(file: File): Observable<string> {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post<string>(this.api+'/User/profile/image/upload', formData, {
      headers: new HttpHeaders().set('Content-Disposition', 'multipart/form-data')
    })
  }

  public createEmailConfirmationRequest(): Observable<void>{
    return this.http.post<void>(`${this.api}/User/profile/email/confirm/request`, null);
  }

  public confirmEmail(code: string): Observable<void>{
    return this.http.post<void>(`${this.api}/User/profile/email/confirm?code=${code}`, null);
  }
}
