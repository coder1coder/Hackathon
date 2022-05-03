import * as signalR from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {HubConnection} from "@microsoft/signalr";
import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {BaseCollection} from "../models/BaseCollection";
import {GetListParameters} from "../models/GetListParameters";
import {Notification} from "../models/Notification/Notification";
import {NotificationFilter} from "../models/Notification/NotificationFilter";
import {
  INotificationChangedIntegrationEvent
} from "../models/IntegrationEvent/INotificationChangedIntegrationEvent";

@Injectable({
  providedIn: 'root'
})

export class NotificationService
{
  api = environment.api;
  storage = sessionStorage;

  private _signalR!: HubConnection;

  public onChanged?:(x:INotificationChangedIntegrationEvent) => void

  constructor(private http:HttpClient) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });

    this.initSignalR();
  }

  markAsRead(ids:string[]):Observable<any>{
    return this.http.post(this.api + '/notification/read', ids);
  }

  getNotifications(filter:GetListParameters<NotificationFilter>):Observable<BaseCollection<Notification>>{
    return this.http.post<BaseCollection<Notification>>(this.api + "/notification/list", filter);
  }

  getUnreadNotificationsCount():Observable<number>{
    return this.http.get<number>(this.api + "/notification/unread/count");
  }

  remove(ids?:string[]):Observable<any>{
    return this.http.request('delete', this.api + "/notification/remove", { body: ids});
  }

  initSignalR(){
    this._signalR = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.notification)
      .build();

    this._signalR.onclose(()=>this.startConnection());

    this._signalR.on("NotificationChanged", (x: INotificationChangedIntegrationEvent) => {

      if (this.onChanged)
        this.onChanged(x)

    });

    this.startConnection();
  }

  startConnection() {
    try {
      this._signalR.start().then(_=>{
        console.log("SignalR Connected.");
      });
    } catch (err) {
      console.log(err);
      setTimeout(this.startConnection, 5000);
    }
  }
}
