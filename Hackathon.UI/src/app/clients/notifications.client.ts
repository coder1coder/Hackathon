import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { GetListParameters } from '../models/GetListParameters';
import { Notification } from '../models/Notification/Notification';
import { NotificationFilter } from '../models/Notification/NotificationFilter';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class NotificationsClient extends BaseApiClient {

  constructor(protected http: HttpClient) {
    super(http, 'notification');
  }

  public markAsRead(ids: string[]): Observable<void> {
    return this.http.post<void>(this.baseRoute + '/read', ids);
  }

  public getNotifications(
    filter: GetListParameters<NotificationFilter>,
  ): Observable<BaseCollection<Notification>> {
    return this.http.post<BaseCollection<Notification>>(this.baseRoute + '/list', filter);
  }

  public getUnreadNotificationsCount(): Observable<number> {
    return this.http.get<number>(this.baseRoute + '/unread/count');
  }

  public remove(ids?: string[]): Observable<void> {
    return this.http.request<void>('delete', this.baseRoute + '/remove', { body: ids });
  }
}
