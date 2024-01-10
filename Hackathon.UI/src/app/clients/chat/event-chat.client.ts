import { HttpClient } from '@angular/common/http';
import { BaseApiClient } from '../base.client';
import { Observable } from 'rxjs';
import { BaseCollection } from '../../models/BaseCollection';
import { Injectable } from '@angular/core';
import { EventChatMessage } from '../../models/chat/EventChatMessage';

@Injectable({
  providedIn: 'root',
})
export class EventChatClient extends BaseApiClient {
  baseRoute = `${this.api}/chat/event`;

  protected constructor(http: HttpClient) {
    super(http);
  }

  public sendAsync(message: EventChatMessage): Observable<void> {
    return this.http.post<void>(this.baseRoute + `/send`, message);
  }

  public getAsync(messageId: string): Observable<EventChatMessage> {
    return this.http.get<EventChatMessage>(`${this.baseRoute}/messages/${messageId}`);
  }

  public getListAsync(
    eventId: number,
    offset: number = 0,
    limit: number = 300,
  ): Observable<BaseCollection<EventChatMessage>> {
    return this.http.post<BaseCollection<EventChatMessage>>(
      this.baseRoute + `/${eventId}/list?offset=${offset}&limit=${limit}`,
      null,
    );
  }
}
