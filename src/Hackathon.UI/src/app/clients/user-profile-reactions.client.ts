import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, switchMap } from 'rxjs';
import { UserProfileReaction, IUserProfileReaction } from '../models/User/UserProfileReaction';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class UserProfileReactionsClient extends BaseApiClient {

  constructor(protected http: HttpClient) {
    super(http, null);
  }

  public get(targetUserId: number): Observable<UserProfileReaction> {
    return this.http.get<UserProfileReaction>(`${this.baseRoute}/User/${targetUserId}/reactions`);
  }

  public getCount(targetUserId: number): Observable<IUserProfileReaction[]> {
    return this.http.get<IUserProfileReaction[]>(
      `${this.baseRoute}/User/${targetUserId}/reactions/count`,
    );
  }

  public toggleReaction(targetUserId: number, reaction: UserProfileReaction): Observable<void> {
    return this.get(targetUserId).pipe(
      switchMap((r: UserProfileReaction) => {
        return (r & reaction) === reaction
          ? this.http.delete<void>(`${this.baseRoute}/User/${targetUserId}/reactions/${reaction}`)
          : this.http.post<void>(`${this.baseRoute}/User/${targetUserId}/reactions/${reaction}`, null);
      }),
    );
  }
}
