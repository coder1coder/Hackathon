import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {EventModel} from "../models/EventModel";
import {PageSettings} from "../models/PageSettings";
import {CreateEvent} from "../models/Event/CreateEvent";
import {BaseCreateResponse} from "../models/BaseCreateResponse";
import {EventStatus} from "../models/EventStatus";

@Injectable({
  providedIn: 'root'
})

export class EventService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient) {

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

  create(createEvent:CreateEvent):Observable<BaseCreateResponse>{
    return this.http.post<BaseCreateResponse>(this.api + "/Event",createEvent);
  }

  setStatus(eventId:number, status:EventStatus) {
    return this.http.put(this.api + "/Event/SetStatus", {
      id: eventId,
      status: status
    });
  }

  join(eventId:number){
    return this.http.post(`${this.api}/Event/${eventId}/Join`, {})
  }

}
