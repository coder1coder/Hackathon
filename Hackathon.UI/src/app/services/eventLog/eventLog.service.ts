import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GetListParameters } from '../../models/GetListParameters';
import { Observable } from 'rxjs';
import { BaseCollection } from '../../models/BaseCollection';
import { IEventLogModel } from '../../models/EventLog/IEventLogModel';

@Injectable({
  providedIn: 'root',
})
export class EventLogService {
  private api: string = `${environment.api}/EventLog`;

  constructor(private http: HttpClient) {
    const headers: HttpHeaders = new HttpHeaders().set('content-type', 'application/json');

    http.options(this.api, {
      headers: headers,
    });
  }

  public getList(
    getFilterModel: GetListParameters<IEventLogModel>,
  ): Observable<BaseCollection<IEventLogModel>> {
    return this.http.post<BaseCollection<IEventLogModel>>(`${this.api}/list`, getFilterModel);
  }
}
