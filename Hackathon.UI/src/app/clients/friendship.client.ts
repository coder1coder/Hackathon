import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseCollection } from 'src/app/models/BaseCollection';
import { GetListParameters } from '../models/GetListParameters';
import { Observable } from 'rxjs';
import { FriendshipFilter } from '../models/Friendship/FriendshipFilter';
import { FriendshipStatus, IFriendship } from '../models/Friendship/FriendshipStatus';
import { IUser } from '../models/User/IUser';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class FriendshipClient extends BaseApiClient {

  constructor(protected http: HttpClient) {
    super(http, 'friendship');
  }

  getUsersByFriendshipStatus(
    userId: number,
    status: FriendshipStatus,
  ): Observable<BaseCollection<IUser>> {
    return this.http.get<BaseCollection<IUser>>(
      `${this.baseRoute}/users?userId=${userId}&status=${status}`,
    );
  }

  getOffers(
    parameters: GetListParameters<FriendshipFilter>,
  ): Observable<BaseCollection<IFriendship>> {
    return this.http.post<BaseCollection<IFriendship>>(`${this.baseRoute}/offers/list`, parameters);
  }

  createOrAcceptOffer(userId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/offer/${userId}`, null);
  }

  unsubscribe(userId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/unsubscribe/${userId}`, null);
  }

  rejectOffer(proposerId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/offer/reject/${proposerId}`, null);
  }

  endFriendship(userId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseRoute}/${userId}`);
  }
}
