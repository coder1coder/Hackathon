import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { BaseCollectionModel } from "../models/BaseCollectionModel";
import { CreateEvent } from "../models/Event/CreateEvent";
import { UpdateEvent } from "../models/Event/UpdateEvent";
import { BaseCreateResponse } from "../models/BaseCreateResponse";
import { EventStatus } from "../models/EventStatus";
import { AuthService } from "./auth.service";
import { GetFilterModel } from "../models/GetFilterModel";
import { EventFilterModel } from "../models/Event/EventFilterModel";
import { EventModel } from "../models/Event/EventModel";

@Injectable({
  providedIn: 'root'
})

export class EventService {
  private api: string = environment.api;
  private storage: Storage = sessionStorage;

  constructor(
    private http: HttpClient,
    private authService: AuthService
    ) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });
  }

  public getAll(params?:GetFilterModel<EventFilterModel>): Observable<BaseCollectionModel<EventModel>> {
    return this.http.post<BaseCollectionModel<EventModel>>(`${this.api}/event/list`, params);
  }

  public getById(eventId:number): Observable<EventModel> {
    return this.http.get<EventModel>(`${this.api}/event/${eventId}`);
  }

  public create(createEvent:CreateEvent): Observable<BaseCreateResponse> {
    return this.http.post<BaseCreateResponse>(`${this.api}/event`, createEvent);
  }

  public update(updateEvent:UpdateEvent): Observable<any> {
    return this.http.put<any>(`${this.api}/event`, updateEvent);
  }

  public remove(eventId:number): Observable<any> {
    return this.http.delete(`${this.api}/event/${eventId}`);
  }

  public setStatus(eventId:number, status:EventStatus) {
    return this.http.put(`${this.api}/event/SetStatus`, {
      id: eventId,
      status: status
    });
  }

  public join(eventId:number){
    return this.http.post(`${this.api}/Event/${eventId}/Join`, {})
  }

  public leave(eventId:number){
    return this.http.post(`${this.api}/Event/${eventId}/leave`, {});
  }

  public isCanJoinToEvent(event:EventModel): boolean {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && !this.isAlreadyInEvent(event, userId)
      && event.status == EventStatus.Published
  }

  public isCanDeleteEvent(event:EventModel): boolean {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && this.isEventOwner(event)
      && event.status == EventStatus.Draft
  }

  public isCanFinishEvent(event:EventModel): boolean {
    return this.isEventOwner(event)
      && event.status != EventStatus.Finished
      && event.status != EventStatus.Draft
  }

  public isCanPublishEvent(event:EventModel): boolean {
    return this.isEventOwner(event)
      && event.status == EventStatus.Draft;
  }

  public isCanStartEvent(event:EventModel): boolean {
    //TODO: check members count
    //TODO: check teams count (auto & manual added)

    return this.isEventOwner(event)
      && event.status == EventStatus.Published;
  }

  public isCanLeave(event:EventModel): boolean {
    let userId = this.authService.getUserId();
    return event.status != EventStatus.Finished
      && userId !== undefined
      && this.isAlreadyInEvent(event, userId);
  }

  public isCanAddTeam(event:EventModel): boolean {
    return this.isEventOwner(event)
      && event.status == EventStatus.Published
      && !event.isCreateTeamsAutomatically
  }

  public isAlreadyInEvent(event:EventModel, userId:number): boolean {
    return event.teamEvents?.filter(t => t.team
        .users?.filter(u => u.id == userId)
        .length > 0
      ).length > 0;
  }

  public isEventOwner(event:EventModel): boolean {
    return event.userId == this.authService.getUserId();
  }

}
