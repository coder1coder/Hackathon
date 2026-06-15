import { Injectable, inject } from '@angular/core';
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

  constructor() {
    super('eventLog');
  }

  public getList(
    getFilterModel: GetListParameters<IEventLogModel>,
  ): Observable<BaseCollection<IEventLogModel>> {
    return this.http.post<BaseCollection<IEventLogModel>>(`${this.baseRoute}/list`, getFilterModel);
  }
}
