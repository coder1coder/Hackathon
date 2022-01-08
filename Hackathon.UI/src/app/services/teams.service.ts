import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {TeamModel} from "../models/Team/TeamModel";
import {Observable} from "rxjs";
import {Router} from "@angular/router";
import {CreateTeamModel} from "../models/Team/CreateTeamModel";
import {BaseCreateResponse} from "../models/BaseCreateResponse";

@Injectable({
  providedIn: 'root'
})

export class TeamsService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient, private router: Router) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  create(createTeamModel:CreateTeamModel): Observable<BaseCreateResponse>{
    return this.http.post<BaseCreateResponse>(this.api+'/Team', createTeamModel);
  }

  getAll():Observable<BaseCollectionModel<TeamModel>>{
    return this.http.get<BaseCollectionModel<TeamModel>>(this.api+'/Team');
  }

  getById(eventId:number){
    return this.http.get<TeamModel>(this.api+'/Team/'+eventId);
  }

  errorHandler(error: HttpErrorResponse) {
    if (error.status == 401)
      this.router.navigate(['/login']);
  }

}
