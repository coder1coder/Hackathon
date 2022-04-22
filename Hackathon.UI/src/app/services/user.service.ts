import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { BaseCollectionModel } from "../models/BaseCollectionModel";
import { UserModel } from "../models/User/UserModel";
import { PageSettings } from "../models/PageSettings";

@Injectable({
  providedIn: 'root'
})

export class UserService {

  private api: string = environment.api;
  private storage: Storage = sessionStorage;

  constructor(private http: HttpClient) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  public getAll(pageSettings?:PageSettings):Observable<BaseCollectionModel<UserModel>>{
    let endpoint = this.api+'/User';
    if (pageSettings != undefined)
      endpoint += `?${pageSettings.toQueryArgs()}`;

    return this.http.get<BaseCollectionModel<UserModel>>(endpoint);
  }

  public getById(userId:number): Observable<UserModel> {
    return this.http.get<UserModel>(this.api+'/User/'+ userId);
  }

  public getProfileImage(id: string): Observable<ArrayBuffer> {
    return this.http.get(this.api + `/filestorage/get/${id}`, { responseType: 'arraybuffer' });
  }
}
