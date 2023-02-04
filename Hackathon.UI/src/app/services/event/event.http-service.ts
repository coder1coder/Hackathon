import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from "../../../environments/environment";
import {Observable} from "rxjs";
import {BaseCollection} from "../../models/BaseCollection";
import {ICreateEvent} from "../../models/Event/ICreateEvent";
import {IUpdateEvent} from "../../models/Event/IUpdateEvent";
import {IBaseCreateResponse} from "../../models/IBaseCreateResponse";
import {EventStatus} from "../../models/Event/EventStatus";
import {GetListParameters} from "../../models/GetListParameters";
import {EventFilter} from "../../models/Event/EventFilter";
import {Event} from "../../models/Event/Event";
import {IEventListItem} from "../../models/Event/IEventListItem";

@Injectable({
  providedIn: 'root'
})
export class EventHttpService {
  private api: string = environment.api;

  constructor(
    private http: HttpClient,
  ) {
    const headers = new HttpHeaders()
      .set('content-type', 'application/json');
    http.options(this.api, {
      'headers': headers
    });
  }

  public getList(params?: GetListParameters<EventFilter>): Observable<BaseCollection<IEventListItem>>{
    let endpoint = this.api+'/event/list';
    return this.http.post<BaseCollection<IEventListItem>>(endpoint, params);
  }

  public getById(eventId: number): Observable<Event> {
    return this.http.get<Event>(this.api+'/Event/'+eventId);
  }

  public create(createEvent: ICreateEvent): Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(this.api + "/Event",createEvent);
  }

  public update(updateEvent: IUpdateEvent): Observable<void> {
    return this.http.put<void>(this.api + "/Event", updateEvent);
  }

  public remove(eventId: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/Event/${eventId}`);
  }

  public setStatus(eventId: number, status: EventStatus): Observable<void> {
    return this.http.put<void>(this.api + "/Event/SetStatus", {
      id: eventId,
      status: status
    });
  }

  public join(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/Event/${eventId}/Join`, {})
  }

  public leave(eventId:number): Observable<void> {
    return this.http.post<void>(`${this.api}/Event/${eventId}/leave`, {});
  }
}