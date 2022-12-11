import {Injectable} from "@angular/core";
import {environment} from "../../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import { BaseCollection } from "src/app/models/BaseCollection";
import {GetListParameters} from "../../models/GetListParameters";
import {Observable} from "rxjs";
import {FriendshipFilter} from "../../models/Friendship/FriendshipFilter";
import {FriendshipStatus, IFriendship} from "../../models/Friendship/FriendshipStatus";
import {HubConnection} from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";
import {
  FriendshipChangedIntegrationEvent
} from "../../models/IntegrationEvent/IFriendshipChangedIntegrationEvent";
import {IUser} from "../../models/User/IUser";

@Injectable({
  providedIn: 'root'
})
export class FriendshipService {
  api = `${environment.api}/Friendship`;
  storage = sessionStorage;

  private _signalR!: HubConnection;
  public onChanged?:(x:FriendshipChangedIntegrationEvent) => void

  constructor(private http: HttpClient) {

    const headers = new HttpHeaders()
      .set('content-type', 'application/json');

    http.options(this.api, {
      'headers': headers
    });

    this.initSignalR();
  }

  getUsersByFriendshipStatus(userId: number, status: FriendshipStatus): Observable<BaseCollection<IUser>>
  {
    return this.http.get<BaseCollection<IUser>>(`${this.api}/users?userId=${userId}&status=${status}`);
  }

  getOffers(parameters: GetListParameters<FriendshipFilter>): Observable<BaseCollection<IFriendship>> {
    return this.http.post<BaseCollection<IFriendship>>(`${this.api}/offers/list`, parameters);
  }

  createOrAcceptOffer(userId: number): Observable<void>
  {
    return this.http.post<void>(`${this.api}/offer/${userId}`, null);
  }

  unsubscribe(userId: number): Observable<void>
  {
    return this.http.post<void>(`${this.api}/unsubscribe/${userId}`, null);
  }

  rejectOffer(proposerId: number): Observable<void>
  {
    return this.http.post<void>(`${this.api}/offer/reject/${proposerId}`, null);
  }

  endFriendship(userId: number): Observable<void>
  {
    return this.http.delete<void>(`${this.api}/${userId}`);
  }

  initSignalR(){
    this._signalR = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.friendship)
      .build();

    this._signalR.onclose(()=>this.startConnection());

    this._signalR.on("FriendshipChanged", (data) => {

      if (this.onChanged)
      {
        let integrationEvent = Object.assign(new FriendshipChangedIntegrationEvent(), JSON.parse(data))
        this.onChanged(integrationEvent)
      }

    });

    this.startConnection();
  }

  startConnection() {
    try {
      this._signalR.start().then(_=>{
        console.log("FriendshipService SignalR Connected.");
      });
    } catch (err) {
      console.log(err);
      setTimeout(this.startConnection, 5000);
    }
  }

}
