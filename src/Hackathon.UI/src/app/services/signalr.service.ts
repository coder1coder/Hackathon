import * as signalR from '@microsoft/signalr';
import { Injectable } from '@angular/core';
import { IEventChatNewMessageIntegrationEvent } from '../models/chat/integrationEvents/IEventChatNewMessageIntegrationEvent';
import { ITeamChatNewMessageIntegrationEvent } from '../models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent';
import { INotificationChangedIntegrationEvent } from '../models/IntegrationEvent/INotificationChangedIntegrationEvent';
import { FriendshipChangedIntegrationEvent } from '../models/IntegrationEvent/IFriendshipChangedIntegrationEvent';
import { IEventStageChangedIntegrationEvent } from '../models/IntegrationEvent/IEventStageChangedIntegrationEvent';
import { IHttpConnectionOptions } from '@microsoft/signalr';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private _connection: signalR.HubConnection;
  private connectionTimeout: number;

  public onEventChatNewMessage: (eventChatNewMessage: IEventChatNewMessageIntegrationEvent) => void;
  public onTeamChatNewMessage: (teamChatNewMessage: ITeamChatNewMessageIntegrationEvent) => void;
  public onNotificationChanged: (notificationChanged: INotificationChangedIntegrationEvent) => void;
  public onFriendshipChangedIntegration: (
    friendshipChangedIntegration: FriendshipChangedIntegrationEvent,
  ) => void;
  public onEventStageChanged: (integrationEvent: IEventStageChangedIntegrationEvent) => void;

  constructor(private authService: AuthService) {}

  public initSignalR(hubUrl: string): void {
    const options: IHttpConnectionOptions = {
      headers: {
        ['Authorization']: `Bearer ${this.authService.getTokenInfo()?.token}`,
      },
      accessTokenFactory: async (): Promise<string> => {
        return this.authService.getTokenInfo()?.token;
      },
    };

    this._connection = new signalR.HubConnectionBuilder().withUrl(hubUrl, options).build();

    this._connection.onclose(() => this.startConnection());

    //TODO: вынести подписки на сообщения в зависимости от регистрируемого хаба

    this._connection.on(
      'EventStageChanged',
      (integrationEvent: IEventStageChangedIntegrationEvent) => {
        if (this.onEventStageChanged) {
          this.onEventStageChanged(integrationEvent);
        }
      },
    );

    this._connection.on(
      'EventChatNewMessage',
      (eventChatNewMessage: IEventChatNewMessageIntegrationEvent) => {
        if (this.onEventChatNewMessage) {
          this.onEventChatNewMessage(eventChatNewMessage);
        }
      },
    );

    this._connection.on(
      'TeamChatNewMessage',
      (teamChatNewMessage: ITeamChatNewMessageIntegrationEvent) => {
        if (this.onTeamChatNewMessage) {
          this.onTeamChatNewMessage(teamChatNewMessage);
        }
      },
    );

    this._connection.on(
      'NotificationChanged',
      (notificationChanged: INotificationChangedIntegrationEvent) => {
        if (this.onNotificationChanged) {
          this.onNotificationChanged(notificationChanged);
        }
      },
    );

    this._connection.on('FriendshipChanged', (friendshipEvent) => {
      if (this.onFriendshipChangedIntegration) {
        const integrationEvent: FriendshipChangedIntegrationEvent = Object.assign(
          new FriendshipChangedIntegrationEvent(),
          JSON.parse(friendshipEvent),
        );
        this.onFriendshipChangedIntegration(integrationEvent);
      }
    });

    this.startConnection();
  }

  private startConnection(): void {
    try {
      this._connection.start().then(() => {
        console.log(`SignalR Connected to ${this._connection.baseUrl}`);
        this.clearConnectionTimeout();
      });
    } catch (err) {
      console.log(err);
      this.connectionTimeout = 5000;
    }
  }

  private clearConnectionTimeout(): void {
    if (this.connectionTimeout) {
      clearTimeout(this.connectionTimeout);
      this.connectionTimeout = null;
    }
  }
}
