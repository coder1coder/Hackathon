import * as signalR from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {HubConnection} from "@microsoft/signalr";
import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Observable} from "rxjs";
import {BaseCollectionModel} from "../models/BaseCollectionModel";
import {NotificationModel} from "../models/Notification/NotificationModel";
import {GetFilterModel} from "../models/GetFilterModel";
import {NotificationFilterModel} from "../models/Notification/NotificationFilterModel";

@Injectable({
  providedIn: 'root'
})

export class NotificationService
{
  api = environment.api;
  storage = sessionStorage;

  private _signalR!: HubConnection;

  public onPublished?:(x:any) => void

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

  getNotifications(filter:GetFilterModel<NotificationFilterModel>):Observable<BaseCollectionModel<NotificationModel>>{
    return this.http.post<BaseCollectionModel<NotificationModel>>(this.api + "/notification/list", filter);
  }

  getUnreadNotificationsCount():Observable<number>{
    return this.http.get<number>(this.api + "/notification/unread/count");
  }

  remove(ids?:string[]):Observable<any>{
    return this.http.request('delete', this.api + "/notification/remove", { body: ids});
  }

  initSignalR(){
    this._signalR = new signalR.HubConnectionBuilder()
      .withUrl(environment.messageHub)
      .build();

    this._signalR.onclose(()=>this.startConnection());

    this._signalR.on("NotificationPublished", (x) => {

      if (this.onPublished)
        this.onPublished(x)

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
