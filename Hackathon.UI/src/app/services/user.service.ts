import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { GetListParameters } from '../models/GetListParameters';
import { UserFilter } from '../models/User/UserFilter';
import { IUpdateUser, IUser } from '../models/User/IUser';
import { FileUploadService } from './file-upload.service';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private api: string = `${environment.api}/user`;

  constructor(private http: HttpClient, private fileUploadService: FileUploadService) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }

  public getList(getFilterModel: GetListParameters<UserFilter>): Observable<BaseCollection<IUser>> {
    return this.http.post<BaseCollection<IUser>>(this.api + '/list', getFilterModel);
  }

  public getById(userId: number): Observable<IUser> {
    return this.http.get<IUser>(`${this.api}/${userId}`);
  }

  /** Загрузить изображение профиля
   * @param files Объекты типа элемента HTML input type="file"
   */
  public setImage(files: FileList): Observable<string> {
    return this.fileUploadService.uploadFile(files, '/user/profile/image/upload');
  }

  public createEmailConfirmationRequest(): Observable<void> {
    return this.http.post<void>(`${this.api}/profile/email/confirm/request`, null);
  }

  public confirmEmail(code: string): Observable<void> {
    return this.http.post<void>(`${this.api}/profile/email/confirm?code=${code}`, null);
  }

  public updateUser(request: IUpdateUser): Observable<void> {
    return this.http.put<void>(`${this.api}/profile/update`, request);
  }
}
