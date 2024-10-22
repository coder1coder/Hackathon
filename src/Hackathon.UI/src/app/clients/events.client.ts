import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { ICreateEvent } from '../models/Event/ICreateEvent';
import { IUpdateEvent } from '../models/Event/IUpdateEvent';
import { IBaseCreateResponse } from '../models/IBaseCreateResponse';
import { EventStatus } from '../models/Event/EventStatus';
import { GetListParameters } from '../models/GetListParameters';
import { EventFilter } from '../models/Event/EventFilter';
import { Event } from '../models/Event/Event';
import { IEventListItem } from '../models/Event/IEventListItem';
import { FileUploadService } from '../services/file-upload.service';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class EventsClient extends BaseApiClient{

  constructor(protected http: HttpClient, private fileUploadService: FileUploadService) {
    super(http, 'event');
  }

  public getList(
    params?: GetListParameters<EventFilter>,
  ): Observable<BaseCollection<IEventListItem>> {
    return this.http.post<BaseCollection<IEventListItem>>(`${this.baseRoute}/list`, params);
  }

  public getById(eventId: number): Observable<Event> {
    return this.http.get<Event>(`${this.baseRoute}/${eventId}`);
  }

  public create(createEvent: ICreateEvent): Observable<IBaseCreateResponse> {
    return this.http.post<IBaseCreateResponse>(this.baseRoute, createEvent);
  }

  public update(updateEvent: IUpdateEvent): Observable<void> {
    return this.http.put<void>(this.baseRoute, updateEvent);
  }

  public remove(eventId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseRoute}/${eventId}`);
  }

  public setStatus(eventId: number, status: EventStatus): Observable<void> {
    return this.http.put<void>(`${this.baseRoute}/setStatus`, {
      id: eventId,
      status: status,
    });
  }

  public acceptAgreement(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${eventId}/agreement/accept`, {});
  }

  public join(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${eventId}/Join`, {});
  }

  public leave(eventId: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${eventId}/leave`, {});
  }

  /** Загрузить изображение события
   * @param files Объекты типа элемента HTML input type="file"
   */
  public setEventImage(files: FileList): Observable<string> {
    return this.fileUploadService.uploadFile(files, '/Event/image/upload');
  }
}
