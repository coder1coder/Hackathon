import * as signalR from "@microsoft/signalr";
import {HubConnection} from "@microsoft/signalr";
import {Injectable} from "@angular/core";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {ChatMessage} from "../../components/team/chat/team.chat.component";
import {Observable} from "rxjs";
import {BaseCollectionModel} from "../../models/BaseCollectionModel";

@Injectable({
  providedIn: 'root'
})

export class ChatService
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

  sendTeamMessage(message:ChatMessage){
    return this.http.post(this.api + `/chat/team/${message.teamId}/send`, message);
  }

  public getTeamMessages(teamId:number, offset:number = 0, limit:number = 300): Observable<BaseCollectionModel<ChatMessage>> {
    return this.http.post<BaseCollectionModel<ChatMessage>>(this.api + `/chat/team/${teamId}/list?offset=${offset}&limit=${limit}`, null);
  }

  initSignalR(){
    this._signalR = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.chat)
      .build();

    this._signalR.onclose(()=>this.startConnection());

    this._signalR.on("ChatMessageChanged", (x) => {

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