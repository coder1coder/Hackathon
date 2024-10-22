import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GetListParameters } from '../models/GetListParameters';
import { Observable } from 'rxjs';
import { BaseCollection } from '../models/BaseCollection';
import { IEventLogModel } from '../models/EventLog/IEventLogModel';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class LogbookClient extends BaseApiClient {

  constructor(protected http: HttpClient) {
    super(http, 'eventLog');
  }

  public getList(
    getFilterModel: GetListParameters<IEventLogModel>,
  ): Observable<BaseCollection<IEventLogModel>> {
    return this.http.post<BaseCollection<IEventLogModel>>(`${this.baseRoute}/list`, getFilterModel);
  }
}
