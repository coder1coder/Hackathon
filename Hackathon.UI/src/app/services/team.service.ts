import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollection} from "../models/BaseCollection";
import {Team} from "../models/Team/Team";
import {Observable, Subject} from "rxjs";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {TeamFilter} from '../models/Team/TeamFilter';
import { GetListParameters } from '../models/GetListParameters';
import {RouterService} from "./router.service";
import {Endpoints} from "../common/Endpoints";

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  api = environment.api;
  storage = sessionStorage;
  constructor(private http: HttpClient,
              private router: RouterService,
              private endpoints: Endpoints) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  create(createTeamModel:CreateTeamModel): Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(this.endpoints.Team.Create, createTeamModel);
  }

  getById(id:number){
    return this.http.get<Team>(this.endpoints.Team.GetById(id));
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status == 401)
      this.router.Profile.Login();
  }

  getMyTeam(): Observable<Team> {
   return this.http.get<Team>(this.endpoints.Team.GetMy);
  }

  getByFilter(getFilterModel: GetListParameters<TeamFilter>):Observable<BaseCollection<Team>>
  {
    return this.http.post<BaseCollection<Team>>(this.endpoints.Team.GetTeams, getFilterModel);
  }

  leaveTeam(id: number):Observable<any> {
    return this.http.get(`${this.api}/team/${id}/leave`);
  }
}
