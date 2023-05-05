import * as signalR from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {HubConnection} from "@microsoft/signalr";
import {Injectable} from "@angular/core";

@Injectable({ providedIn: 'root' })
export class SignalRService
{
  private _connection!: HubConnection;

  public onChatMessageChanged?:(x:any) => void

  constructor() {
    this.initSignalR();
  }

  initSignalR(){
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.chat)
      .build();

    this._connection.onclose(()=>this.startConnection());

    this._connection.on("ChatMessageChanged", (x) => {

      if (this.onChatMessageChanged)
        this.onChatMessageChanged(x)

    });

    this.startConnection();
  }

  startConnection() {
    try
    {
      this._connection.start().then(_=>{
        console.debug("SignalR Connected");
      });
    }
    catch (err)
    {
      console.log(err);
      setTimeout(this.startConnection, 5000);
    }
  }
}
