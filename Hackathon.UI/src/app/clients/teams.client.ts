import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
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
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class TeamsClient extends BaseApiClient {

  constructor(protected http: HttpClient) {
    super(http, 'team');
  }

  public create(createTeamModel: CreateTeamModel): Observable<IBaseCreateResponse> {
    return this.http.post<IBaseCreateResponse>(this.baseRoute, createTeamModel);
  }

  public getById(id: number): Observable<Team> {
    return this.http.get<Team>(`${this.baseRoute}/${id}`);
  }

  public getMyTeam(): Observable<Team> {
    return this.http.get<Team>(`${this.baseRoute}/my`);
  }

  public getByFilter(
    getFilterModel: GetListParameters<TeamFilter>,
  ): Observable<BaseCollection<Team>> {
    return this.http.post<BaseCollection<Team>>(`${this.baseRoute}/getTeams`, getFilterModel);
  }

  public joinToTeam(teamId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${teamId}/join`, null);
  }

  public leaveTeam(teamId: number): Observable<void> {
    return this.http.get<void>(`${this.baseRoute}/${teamId}/leave`);
  }

  public createJoinRequest(teamId: number): Observable<{ id: number }> {
    return this.http.post<{ id: number }>(`${this.baseRoute}/${teamId}/join/request`, null);
  }

  public approveJoinRequest(requestId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/join/request/${requestId}/approve`, null);
  }

  public cancelJoinRequest(parameters: ICancelRequestParameters): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/join/request/cancel`, parameters);
  }

  public getSentJoinRequest(teamId: number): Observable<ITeamJoinRequest> {
    return this.http.get<ITeamJoinRequest>(`${this.baseRoute}/${teamId}/join/request/sent`);
  }

  public getJoinRequests(
    filter: GetListParameters<ITeamJoinRequestFilter>,
  ): Observable<BaseCollection<ITeamJoinRequest>> {
    return this.http.post<BaseCollection<ITeamJoinRequest>>(
      `${this.baseRoute}/join/request/list`,
      filter,
    );
  }

  public getTeamSentJoinRequests(
    teamId: number,
    paginationSorting: PaginationSorting,
  ): Observable<BaseCollection<ITeamJoinRequest>> {
    return this.http.post<BaseCollection<ITeamJoinRequest>>(
      `${this.baseRoute}/${teamId}/join/request/list`,
      paginationSorting,
    );
  }
}
