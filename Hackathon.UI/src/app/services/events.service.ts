import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {EventModel} from "../models/EventModel";
import {PageSettings} from "../models/PageSettings";

@Injectable({
  providedIn: 'root'
})

export class EventsService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient, private router: Router) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  getAll(pageSettings?:PageSettings):Observable<BaseCollectionModel<EventModel>>{

    let endpoint = this.api+'/Event';

    if (pageSettings != undefined)
      endpoint += `?${pageSettings.toQueryArgs()}`;

    return this.http.get<BaseCollectionModel<EventModel>>(endpoint);
  }

  getById(eventId:number){
    return this.http.get<EventModel>(this.api+'/Event/'+eventId);
  }

}
