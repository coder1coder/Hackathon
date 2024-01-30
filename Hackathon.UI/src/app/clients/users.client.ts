import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { GetListParameters } from '../models/GetListParameters';
import { UserFilter } from '../models/User/UserFilter';
import { IUpdateUser, IUser } from '../models/User/IUser';
import { IUpdatePasswordParameters } from '../models/User/IUpdatePasswordParameters';
import { FileUploadService } from '../services/file-upload.service';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class UsersClient extends BaseApiClient {

  constructor(protected http: HttpClient, private fileUploadService: FileUploadService) {
    super(http, 'user');
  }

  public getList(getFilterModel: GetListParameters<UserFilter>): Observable<BaseCollection<IUser>> {
    return this.http.post<BaseCollection<IUser>>(this.baseRoute + '/list', getFilterModel);
  }

  public getById(userId: number): Observable<IUser> {
    return this.http.get<IUser>(`${this.baseRoute}/${userId}`);
  }

  /** Загрузить изображение профиля
   * @param files Объекты типа элемента HTML input type="file"
   */
  public setImage(files: FileList): Observable<string> {
    return this.fileUploadService.uploadFile(files, '/user/profile/image/upload');
  }

  public createEmailConfirmationRequest(): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/profile/email/confirm/request`, null);
  }

  public confirmEmail(code: string): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/profile/email/confirm?code=${code}`, null);
  }

  public updateUser(request: IUpdateUser): Observable<void> {
    return this.http.put<void>(`${this.baseRoute}/profile/update`, request);
  }

  public updatePassword(parameters: IUpdatePasswordParameters): Observable<void> {
    return this.http.put<void>(`${this.baseRoute}/password`, parameters);
  }
}
