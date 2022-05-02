import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {TeamModel} from "../models/Team/TeamModel";
import {Observable} from "rxjs";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {TeamFilterModel} from '../models/Team/TeamFilterModel';
import { GetFilterModel } from '../models/GetFilterModel';
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
    return this.http.get<TeamModel>(this.endpoints.Team.GetById(id));
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status == 401)
      this.router.Profile.Login();
  }

  getMyTeam = () => this.http.get<TeamModel>(this.endpoints.Team.GetMy);

  getByFilter(getFilterModel: GetFilterModel<TeamFilterModel>):Observable<BaseCollectionModel<TeamModel>>
  {
    return this.http.post<BaseCollectionModel<TeamModel>>(this.endpoints.Team.GetTeams, getFilterModel);
  }
}
