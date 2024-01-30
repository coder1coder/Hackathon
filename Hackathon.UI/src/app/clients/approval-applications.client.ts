import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetListParameters } from '../models/GetListParameters';
import { BaseCollection } from '../models/BaseCollection';
import {
  IApprovalApplication,
  IApprovalApplicationFilter,
} from '../models/approval-application/approval-application.interface';
import { BaseApiClient } from './base.client';

@Injectable({
  providedIn: 'root',
})
export class ApprovalApplicationsClient extends BaseApiClient{

  constructor(http: HttpClient) {
    super(http, 'approvalapplications');
  }

  /** Получить список заявок на согласование
   * @param params Параметры фильтрации и пагинации
   * @returns Параметры фильтрации и пагинации
   */
  public getApprovalApplicationList(
    params?: GetListParameters<IApprovalApplicationFilter>,
  ): Observable<BaseCollection<IApprovalApplication>> {
    return this.http.post<BaseCollection<IApprovalApplication>>(
      `${this.baseRoute}/list`,
      params,
    );
  }

  /** Получить заявку на согласование по идентификатору заявки
   * @param id Идентификатор заявки
   * @returns Заявления на согласование
   */
  public getApprovalApplication(id: number): Observable<IApprovalApplication> {
    return this.http.get<IApprovalApplication>(`${this.baseRoute}/${id}`);
  }

  /** Согласовать заявку на согласование
   * @param id Идентификатор заявки
   * @returns пустой ответ
   */
  public approveApprovalApplication(id: number): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${id}/approve`, null);
  }

  /** Отклонить заявку на согласование
   * @param id Идентификатор заявки
   * @param comment Причина отклонения заявки
   * @returns пустой ответ
   */
  public rejectApprovalApplication(id: number, comment: string): Observable<void> {
    return this.http.post<void>(`${this.baseRoute}/${id}/reject`, { comment });
  }
}
