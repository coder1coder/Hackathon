import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { BaseCollection } from '../models/BaseCollection';
import { Team } from '../models/Team/Team';
import { Observable } from 'rxjs';
import { CreateTeamModel } from '../models/Team/CreateTeamModel';
import { IBaseCreateResponse } from '../models/IBaseCreateResponse';
import { TeamFilter } from '../models/Team/TeamFilter';
import { GetListParameters, PaginationSorting } from '../models/GetListParameters';
import { ITeamJoinRequest } from '../models/Team/ITeamJoinRequest';
import { ITeamJoinRequestFilter } from '../models/Team/ITeamJoinRequestFilter';
import { ICancelRequestParameters } from '../models/Team/CancelRequestParameters';

@Injectable({
  providedIn: 'root',
})
export class TeamClient {
  private api: string = `${environment.api}/team`;

  constructor(private http: HttpClient) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }

  public create(createTeamModel: CreateTeamModel): Observable<IBaseCreateResponse> {
    return this.http.post<IBaseCreateResponse>(this.api, createTeamModel);
  }

  public getById(id: number): Observable<Team> {
    return this.http.get<Team>(`${this.api}/${id}`);
  }

  public getMyTeam(): Observable<Team> {
    return this.http.get<Team>(`${this.api}/My`);
  }

  public getByFilter(
    getFilterModel: GetListParameters<TeamFilter>,
  ): Observable<BaseCollection<Team>> {
    return this.http.post<BaseCollection<Team>>(`${this.api}/getTeams`, getFilterModel);
  }

  public joinToTeam(teamId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/${teamId}/join`, null);
  }

  public leaveTeam(teamId: number): Observable<void> {
    return this.http.get<void>(`${this.api}/${teamId}/leave`);
  }

  public createJoinRequest(teamId: number): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.api}/${teamId}/join/request`, null);
  }

  public approveJoinRequest(requestId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/join/request/${requestId}/approve`, null);
  }

  public cancelJoinRequest(parameters: ICancelRequestParameters): Observable<void> {
    return this.http.post<void>(`${this.api}/join/request/cancel`, parameters);
  }

  public getSentJoinRequest(teamId: number): Observable<ITeamJoinRequest> {
    return this.http.get<ITeamJoinRequest>(`${this.api}/${teamId}/join/request/sent`);
  }

  public getJoinRequests(
    filter: GetListParameters<ITeamJoinRequestFilter>,
  ): Observable<BaseCollection<ITeamJoinRequest>> {
    return this.http.post<BaseCollection<ITeamJoinRequest>>(
      `${this.api}/join/request/list`,
      filter,
    );
  }

  public getTeamSentJoinRequests(
    teamId: number,
    paginationSorting: PaginationSorting,
  ): Observable<BaseCollection<ITeamJoinRequest>> {
    return this.http.post<BaseCollection<ITeamJoinRequest>>(
      `${this.api}/${teamId}/join/request/list`,
      paginationSorting,
    );
  }
}
