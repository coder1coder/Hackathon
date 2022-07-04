import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../environments/environment";
import {Observable} from "rxjs";
import {BaseCollection} from "../models/BaseCollection";
import {ICreateEvent} from "../models/Event/ICreateEvent";
import {IUpdateEvent} from "../models/Event/IUpdateEvent";
import {IBaseCreateResponse} from "../models/IBaseCreateResponse";
import {EventStatus} from "../models/Event/EventStatus";
import {AuthService} from "./auth.service";
import {GetListParameters} from "../models/GetListParameters";
import {EventFilter} from "../models/Event/EventFilter";
import {Event} from "../models/Event/Event";
import {IEventListItem} from "../models/Event/IEventListItem";

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

  getList(params?:GetListParameters<EventFilter>):Observable<BaseCollection<IEventListItem>>{
    let endpoint = this.api+'/event/list';
    return this.http.post<BaseCollection<IEventListItem>>(endpoint, params);
  }

  getById(eventId:number){
    return this.http.get<Event>(this.api+'/Event/'+eventId);
  }

  create(createEvent:ICreateEvent):Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(this.api + "/Event",createEvent);
  }

  update(updateEvent:IUpdateEvent):Observable<any> {
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

  isCanJoinToEvent(event:Event) {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && !this.isAlreadyInEvent(event, userId)
      && event.status == EventStatus.Published
  }

  isCanDeleteEvent(event:Event) {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && this.isEventOwner(event)
      && event.status == EventStatus.Draft
  }

  isCanFinishEvent(event:Event){
    return this.isEventOwner(event)
      && event.status != EventStatus.Finished
      && event.status != EventStatus.Draft
  }

  isCanPublishEvent(event:Event){
    return this.isEventOwner(event)
      && event.status == EventStatus.Draft;
  }

  isCanStartEvent(event:Event){
    //TODO: check members count
    //TODO: check teams count (auto & manual added)

    return this.isEventOwner(event)
      && event.status == EventStatus.Published;
  }

  isCanLeave(event:Event)
  {
    let userId = this.authService.getUserId();
    return event.status != EventStatus.Finished
      && userId !== undefined
      && this.isAlreadyInEvent(event, userId);
  }

  isCanAddTeam(event:Event){
    return this.isEventOwner(event)
      && event.status == EventStatus.Published
      && !event.isCreateTeamsAutomatically
  }

  isAlreadyInEvent(event:Event, userId:number){
    return event.teams?.filter(t => t
        .members?.filter(x => x.id == userId)
        .length > 0
      ).length > 0;
  }

  isEventOwner(event:Event){
    return event.ownerId == this.authService.getUserId();
  }

}
