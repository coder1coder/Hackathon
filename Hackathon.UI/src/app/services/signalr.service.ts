import * as signalR from "@microsoft/signalr";
import {environment} from "../../environments/environment";
import {HubConnection} from "@microsoft/signalr";
import {Injectable} from "@angular/core";
import { IEventChatNewMessageIntegrationEvent } from "../models/chat/integrationEvents/IEventChatNewMessageIntegrationEvent";
import { ITeamChatNewMessageIntegrationEvent } from "../models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent";

@Injectable({ providedIn: 'root' })
export class SignalRService
{
  private _connection!: HubConnection;

  public onEventChatNewMessage?:(x:IEventChatNewMessageIntegrationEvent) => void
  public onTeamChatNewMessage?:(x:ITeamChatNewMessageIntegrationEvent) => void

  constructor() {
    this.initSignalR();
  }

  initSignalR(){
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.chat)
      .build();

    this._connection.onclose(()=>this.startConnection());

    this._connection.on("EventChatNewMessage", (x:IEventChatNewMessageIntegrationEvent) => {
      if (this.onEventChatNewMessage)
        this.onEventChatNewMessage(x)
    });

    this._connection.on("TeamChatNewMessage", (x:ITeamChatNewMessageIntegrationEvent) => {
      if (this.onTeamChatNewMessage)
        this.onTeamChatNewMessage(x)
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
