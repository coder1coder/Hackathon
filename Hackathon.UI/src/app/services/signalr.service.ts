import * as signalR from "@microsoft/signalr";
import { environment } from "../../environments/environment";
import { HttpTransportType } from "@microsoft/signalr";
import { Injectable } from "@angular/core";
import { IEventChatNewMessageIntegrationEvent } from "../models/chat/integrationEvents/IEventChatNewMessageIntegrationEvent";
import { ITeamChatNewMessageIntegrationEvent } from "../models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent";

@Injectable({
  providedIn: 'root'
})
export class SignalRService  {
  private _connection: signalR.HubConnection;
  private connectionTimeout: number;

  public onEventChatNewMessage: (eventChatNewMessage: IEventChatNewMessageIntegrationEvent) => void;
  public onTeamChatNewMessage: (teamChatNewMessage: ITeamChatNewMessageIntegrationEvent) => void;

  constructor() {
    this.initSignalR();
  }

  public initSignalR(): void {
    this._connection = new signalR.HubConnectionBuilder()
      .withUrl(environment.hubs.chat, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
      })
      .build();

    this._connection.onclose(()=> this.startConnection());

    this._connection.on("EventChatNewMessage", (eventChatNewMessage: IEventChatNewMessageIntegrationEvent) => {
      if (this.onEventChatNewMessage) {
        this.onEventChatNewMessage(eventChatNewMessage);
      }
    });

    this._connection.on("TeamChatNewMessage", (teamChatNewMessage: ITeamChatNewMessageIntegrationEvent) => {
      if (this.onTeamChatNewMessage) {
        this.onTeamChatNewMessage(teamChatNewMessage);
      }
    });

    this.startConnection();
  }

  private startConnection(): void {
    try {
      this._connection.start().then(() => {
        console.debug("SignalR Connected");
        this.clearConnectionTimeout();
      });
    } catch (err) {
      console.log(err);
      this.connectionTimeout = setTimeout(this.startConnection, 5000);
    }
  }

  private clearConnectionTimeout(): void {
    if (this.connectionTimeout) {
      clearTimeout(this.connectionTimeout);
      this.connectionTimeout = null;
    }
  }
}
