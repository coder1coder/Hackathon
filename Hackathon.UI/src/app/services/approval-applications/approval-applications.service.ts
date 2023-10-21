import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { GetListParameters } from "../../models/GetListParameters";
import { BaseCollection } from "../../models/BaseCollection";
import {
  IApprovalApplication,
  IApprovalApplicationFilter
} from "../../models/approval-application/approval-application.interface";

@Injectable({
  providedIn: 'root'
})
export class ApprovalApplicationsService {

  private api: string = `${environment.api}`;

  constructor(private http: HttpClient) {}

  /** Получить список заявок на согласование
   * @param params Параметры фильтрации и пагинации
   * @returns Параметры фильтрации и пагинации
   */
  public getApprovalApplicationList(params?: GetListParameters<IApprovalApplicationFilter>): Observable<BaseCollection<IApprovalApplication>> {
    return this.http.post<BaseCollection<IApprovalApplication>>(`${this.api}/approvalapplications/list`, params);
  }

  /** Получить заявку на согласование по идентификатору заявки
   * @param id Идентификатор заявки
   * @returns Заявления на согласование
   */
  public getApprovalApplication(id: number): Observable<IApprovalApplication> {
    return this.http.get<IApprovalApplication>(`${this.api}/approvalapplications/${id}`);
  }

  /** Согласовать заявку на согласование
   * @param id Идентификатор заявки
   * @returns пустой ответ
   */
  public approveApprovalApplication(id: number): Observable<void> {
    return this.http.post<void>(`${this.api}/approvalapplications/${id}/approve`, null);
  }

  /** Отклонить заявку на согласование
   * @param id Идентификатор заявки
   * @param comment Причина отклонения заявки
   * @returns пустой ответ
   */
  public rejectApprovalApplication(id: number, comment: string): Observable<void> {
    return this.http.post<void>(`${this.api}/approvalapplications/${id}/reject`, {comment});
  }
}
