import { environment } from '../../environments/environment';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { GetListParameters } from '../models/GetListParameters';
import { Notification } from '../models/Notification/Notification';
import { NotificationFilter } from '../models/Notification/NotificationFilter';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private api: string = environment.api;

  constructor(private http: HttpClient) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }

  public markAsRead(ids: string[]): Observable<void> {
    return this.http.post<void>(this.api + '/notification/read', ids);
  }

  public getNotifications(
    filter: GetListParameters<NotificationFilter>,
  ): Observable<BaseCollection<Notification>> {
    return this.http.post<BaseCollection<Notification>>(this.api + '/notification/list', filter);
  }

  public getUnreadNotificationsCount(): Observable<number> {
    return this.http.get<number>(this.api + '/notification/unread/count');
  }

  public remove(ids?: string[]): Observable<void> {
    return this.http.request<void>('delete', this.api + '/notification/remove', { body: ids });
  }
}
