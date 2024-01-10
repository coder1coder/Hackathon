import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, switchMap } from 'rxjs';
import { UserProfileReaction, IUserProfileReaction } from '../models/User/UserProfileReaction';

@Injectable({
  providedIn: 'root',
})
export class UserProfileReactionService {
  private api: string = environment.api;

  constructor(private http: HttpClient) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }

  public get(targetUserId: number): Observable<UserProfileReaction> {
    return this.http.get<UserProfileReaction>(`${this.api}/User/${targetUserId}/reactions`);
  }

  public getCount(targetUserId: number): Observable<IUserProfileReaction[]> {
    return this.http.get<IUserProfileReaction[]>(
      `${this.api}/User/${targetUserId}/reactions/count`,
    );
  }

  public toggleReaction(targetUserId: number, reaction: UserProfileReaction): Observable<void> {
    return this.get(targetUserId).pipe(
      switchMap((r: UserProfileReaction) => {
        return (r & reaction) === reaction
          ? this.http.delete<void>(`${this.api}/User/${targetUserId}/reactions/${reaction}`)
          : this.http.post<void>(`${this.api}/User/${targetUserId}/reactions/${reaction}`, null);
      }),
    );
  }
}
