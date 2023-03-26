import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollection} from "../models/BaseCollection";
import {Team} from "../models/Team/Team";
import {Observable} from "rxjs";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {TeamFilter} from '../models/Team/TeamFilter';
import {GetListParameters, PaginationSorting} from '../models/GetListParameters';
import {ITeamJoinRequest} from "../models/Team/ITeamJoinRequest";
import {ITeamJoinRequestFilter} from "../models/Team/ITeamJoinRequestFilter";
import {ICancelRequestParameters} from "../models/Team/CancelRequestParameters";

@Injectable({
  providedIn: 'root'
})
export class TeamClient {
  api = `${environment.api}/team`;
  storage = sessionStorage;
  constructor(private http: HttpClient) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  create(createTeamModel:CreateTeamModel): Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(this.api, createTeamModel);
  }

  getById(id:number){
    return this.http.get<Team>(`${this.api}/${id}`);
  }

  getMyTeam(): Observable<Team> {
   return this.http.get<Team>(`${this.api}/My`);
  }

  getByFilter(getFilterModel: GetListParameters<TeamFilter>):Observable<BaseCollection<Team>>
  {
    return this.http.post<BaseCollection<Team>>(`${this.api}/getTeams`, getFilterModel);
  }

  joinToTeam(teamId: number){
    return this.http.post(`${this.api}/${teamId}/join`, null);
  }

  leaveTeam(teamId: number):Observable<any> {
    return this.http.get(`${this.api}/${teamId}/leave`);
  }

  createJoinRequest(teamId: number) {
    return this.http.post(`${this.api}/${teamId}/join/request`, null);
  }

  approveJoinRequest(requestId:number) {
    return this.http.post(`${this.api}/join/request/${requestId}/approve`, null);
  }

  cancelJoinRequest(parameters: ICancelRequestParameters) {
    return this.http.post(`${this.api}/join/request/cancel`, parameters);
  }

  getSentJoinRequest(teamId: number):Observable<ITeamJoinRequest> {
    return this.http.get<ITeamJoinRequest>(`${this.api}/${teamId}/join/request/sent`);
  }

  getJoinRequests(filter: GetListParameters<ITeamJoinRequestFilter>):Observable<BaseCollection<ITeamJoinRequest>>{
    return this.http.post<BaseCollection<ITeamJoinRequest>>(`${this.api}/join/request/list`, filter);
  }

  getTeamSentJoinRequests(teamId: number, paginationSorting: PaginationSorting):Observable<BaseCollection<ITeamJoinRequest>>{
    return this.http.post<BaseCollection<ITeamJoinRequest>>(`${this.api}/${teamId}/join/request/list`, paginationSorting);
  }
}
