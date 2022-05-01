import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { BaseCollectionModel } from "../models/BaseCollectionModel";
import { PageSettings } from "../models/PageSettings";
import {IUserModel} from "../models/User/IUserModel";

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

  public getAll(pageSettings?:PageSettings):Observable<BaseCollectionModel<IUserModel>>{
    let endpoint = this.api+'/User';
    if (pageSettings != undefined)
      endpoint += `?${pageSettings.toQueryArgs()}`;

    return this.http.get<BaseCollectionModel<IUserModel>>(endpoint);
  }

  public getById(userId:number): Observable<IUserModel> {
    return this.http.get<IUserModel>(this.api+'/User/'+ userId);
  }

  public getProfileImage(id: string): Observable<ArrayBuffer> {
    return this.http.get(this.api + `/filestorage/get/${id}`, { responseType: 'arraybuffer' });
  }

  public setProfileImage(file: File): Observable<string> {
    const formData = new FormData();
    formData.append("file", file);

    return this.http.post<string>(this.api+'/User/profile/image/upload', formData, {
      headers: new HttpHeaders().set('Content-Disposition', 'multipart/form-data')
    })
  }
}
