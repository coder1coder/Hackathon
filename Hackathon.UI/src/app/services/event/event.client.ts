import {HttpClient} from '@angular/common/http';
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
import {FileUploadService} from "../file-upload.service";

@Injectable({
  providedIn: 'root'
})
export class EventClient {
  private api: string = `${environment.api}/event`;

  constructor(
    private http: HttpClient,
    private fileUploadService: FileUploadService
  ) {
  }

  public getList(params?: GetListParameters<EventFilter>): Observable<BaseCollection<IEventListItem>>{
    return this.http.post<BaseCollection<IEventListItem>>(`${this.api}/list`, params);
  }

  public getById(eventId: number): Observable<Event> {
    return this.http.get<Event>(`${this.api}/${eventId}`);
  }

  public create(createEvent: ICreateEvent): Observable<IBaseCreateResponse>{
    return this.http.post<IBaseCreateResponse>(this.api, createEvent);
  }

  public update(updateEvent: IUpdateEvent): Observable<void> {
    return this.http.put<void>(this.api, updateEvent);
  }

  public remove(eventId: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/${eventId}`);
  }

  public setStatus(eventId: number, status: EventStatus): Observable<void> {
    return this.http.put<void>(`${this.api}/setStatus`, {
      id: eventId,
      status: status
    });
  }

  public acceptAgreement(eventId: number): Observable<void>{
    return this.http.post<void>(`${this.api}/${eventId}/agreement/accept`, {});
  }

  public join(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.api}/${eventId}/join`, {})
  }

  public leave(eventId:number): Observable<void> {
    return this.http.post<void>(`${this.api}/${eventId}/leave`, {});
  }

  /** Загрузить изображение события
   * @param files Объекты типа элемента HTML input type="file"
   */
  public setEventImage(files: FileList): Observable<string> {
    return this.fileUploadService.uploadFile(files, '/Event/image/upload');
  }
}
