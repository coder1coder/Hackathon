import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {CreateEvent} from "../models/Event/CreateEvent";
import {UpdateEvent} from "../models/Event/UpdateEvent";
import {BaseCreateResponse} from "../models/BaseCreateResponse";
import {EventStatus} from "../models/EventStatus";
import {AuthService} from "./auth.service";
import {GetFilterModel} from "../models/GetFilterModel";
import {EventFilterModel} from "../models/Event/EventFilterModel";
import {EventModel} from "../models/Event/EventModel";
import {User} from "../models/User";

@Injectable({
  providedIn: 'root'
})

export class EventService {

  api = environment.api;
  storage = sessionStorage;

  constructor(private http: HttpClient, private authService: AuthService) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  getAll(params?:GetFilterModel<EventFilterModel>):Observable<BaseCollectionModel<EventModel>>{

    let endpoint = this.api+'/event/list';
    return this.http.post<BaseCollectionModel<EventModel>>(endpoint, params);
  }

  getById(eventId:number){
    return this.http.get<EventModel>(this.api+'/Event/'+eventId);
  }

  create(createEvent:CreateEvent):Observable<BaseCreateResponse>{
    return this.http.post<BaseCreateResponse>(this.api + "/Event",createEvent);
  }

  update(updateEvent:UpdateEvent):Observable<any> {
    return this.http.put<any>(this.api + "/Event", updateEvent);
  }

  remove(eventId:number){
    return this.http.delete(`${this.api}/Event/${eventId}`);
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

  leave(eventId:number){
    return this.http.post(`${this.api}/Event/${eventId}/leave`, {});
  }

  isCanJoinToEvent(event:EventModel) {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && !this.isAlreadyInEvent(event, userId)
      && event.status == EventStatus.Published
  }

  isCanDeleteEvent(event:EventModel) {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && this.isEventOwner(event)
      && event.status == EventStatus.Draft
  }

  isCanFinishEvent(event:EventModel){
    return this.isEventOwner(event)
      && event.status != EventStatus.Finished
      && event.status != EventStatus.Draft
  }

  isCanPublishEvent(event:EventModel){
    return this.isEventOwner(event)
      && event.status == EventStatus.Draft;
  }

  isCanStartEvent(event:EventModel){
    //TODO: check members count
    //TODO: check teams count (auto & manual added)

    return this.isEventOwner(event)
      && event.status == EventStatus.Published;
  }

  isCanLeave(event:EventModel)
  {
    let userId = this.authService.getUserId();
    return event.status != EventStatus.Finished
      && userId !== undefined
      && this.isAlreadyInEvent(event, userId);
  }

  isCanAddTeam(event:EventModel){
    return this.isEventOwner(event)
      && event.status == EventStatus.Published
      && !event.isCreateTeamsAutomatically
  }

  isAlreadyInEvent(event:EventModel, userId:number){
    return event.teamEvents?.filter(t => t.team
        .users?.filter(u => u.id == userId)
        .length > 0
      ).length > 0;
  }

  isEventOwner(event:EventModel){
    return event.userId == this.authService.getUserId();
  }

}
