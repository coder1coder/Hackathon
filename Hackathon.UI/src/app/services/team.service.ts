import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {TeamModel} from "../models/Team/TeamModel";
import {Observable} from "rxjs";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {BaseCreateResponse} from "../models/BaseCreateResponse";
import {TeamFilterModel} from '../models/Team/TeamFilterModel';
import { GetFilterModel } from '../models/GetFilterModel';
import {RouterService} from "./router.service";

@Injectable({
  providedIn: 'root'
})

export class TeamService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient, private router: RouterService) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  create(createTeamModel:CreateTeamModel): Observable<BaseCreateResponse>{
    return this.http.post<BaseCreateResponse>(this.api+'/Team', createTeamModel);
  }

  getById(eventId:number){
    return this.http.get<TeamModel>(this.api+'/Team/'+eventId);
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status == 401)
      this.router.Profile.Login();
  }

  getMyTeam = () => this.http.get<TeamModel>(`${this.api}/Team/My`);

  getByFilter(getFilterModel: GetFilterModel<TeamFilterModel>):Observable<BaseCollectionModel<TeamModel>>{
    let endpoint = this.api+'/Team/getTeams';
    return this.http.post<BaseCollectionModel<TeamModel>>(endpoint, getFilterModel);
  }
}
