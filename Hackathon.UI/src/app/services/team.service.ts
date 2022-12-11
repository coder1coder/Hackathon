import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollection} from "../models/BaseCollection";
import {Team} from "../models/Team/Team";
import {Observable} from "rxjs";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {TeamFilter} from '../models/Team/TeamFilter';
import { GetListParameters } from '../models/GetListParameters';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  api = environment.api;
  storage = sessionStorage;
  constructor(private http: HttpClient) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  create(createTeamModel:CreateTeamModel): Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(`${this.api}/Team`, createTeamModel);
  }

  getById(id:number){
    return this.http.get<Team>(`${this.api}/Team/${id}`);
  }

  getMyTeam(): Observable<Team> {
   return this.http.get<Team>(`${this.api}/Team/My`);
  }

  getByFilter(getFilterModel: GetListParameters<TeamFilter>):Observable<BaseCollection<Team>>
  {
    return this.http.post<BaseCollection<Team>>(`${this.api}/Team/getTeams`, getFilterModel);
  }

  leaveTeam(id: number):Observable<any> {
    return this.http.get(`${this.api}/team/${id}/leave`);
  }
}
