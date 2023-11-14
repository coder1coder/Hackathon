import { Component, ElementRef, Injectable, Input, ViewChild } from '@angular/core';
import { FormBuilder } from "@angular/forms";
import { AuthService } from "../../../services/auth.service";
import { BaseCollection } from "../../../models/BaseCollection";
import { ChatMessageOption } from "../../../models/chat/TeamChatMessage";
import { BaseChatComponent } from "../base.chat.component";
import { SignalRService } from "../../../services/signalr.service";
import { BehaviorSubject, Observable, of, takeUntil } from "rxjs";
import { IUser } from "../../../models/User/IUser";
import { EventChatMessage } from "../../../models/chat/EventChatMessage";
import { EventChatClient } from "../../../clients/chat/event-chat.client";
import { Event } from "../../../models/Event/Event";
import { EventClient } from "../../../services/event/event.client";
import { IEventChatNewMessageIntegrationEvent } from 'src/app/models/chat/integrationEvents/IEventChatNewMessageIntegrationEvent';
import { tap } from "rxjs/operators";
import { ProfileUserStore } from "../../../shared/stores/profile-user.store";
import { ErrorProcessorService } from "../../../services/error-processor.service";

@Component({
  selector: 'chat-event',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss'],
})

@Injectable()
export class ChatEventComponent extends BaseChatComponent<EventChatMessage> {
  @ViewChild('scrollMe') chatBody: ElementRef;

  @Input() event: Event;
  @Input() showMembers: boolean = false;
  @Input("eventId")
  public set eventId(value) { this.entityId.next(value); };
  public get eventId() { return this.entityId.getValue(); };

  public entityId = new BehaviorSubject<number>(0);
  public messages: EventChatMessage[] = [];

  constructor(
    protected authService: AuthService,
    protected fb: FormBuilder,
    protected profileUserStore: ProfileUserStore,
    private signalRService: SignalRService,
    private eventClient: EventClient,
    private eventChatClient: EventChatClient,
    private errorProcessor: ErrorProcessorService,
  ) {
    super(authService, fb, profileUserStore)
    signalRService.onEventChatNewMessage = (x => this.handleNewMessageEvent(x));
  }

  public handleNewMessageEvent(eventChatNewMessage: IEventChatNewMessageIntegrationEvent): void {
    if (this.canView && this.eventId > 0 && this.eventId == eventChatNewMessage.eventId) {
      this.eventChatClient.getAsync(eventChatNewMessage.messageId)
        .pipe(
          tap((res: EventChatMessage) => {
            this.messages.push(res);
            return res;
          }),
          takeUntil(this.destroy$),
        )
        .subscribe({
          next: () => {
            this.scrollChatToLastMessage();
          },
          error: () => {},
        });
    }
  }

  public fetchEntity(needReload: boolean = false): void {
    const request: Observable<Event> = Boolean(this.event) && !needReload ?
      of(this.event) :
      this.eventClient.getById(this.eventId);

    request
      .pipe(takeUntil(this.destroy$))
      .subscribe((event: Event) => {
        this.event = event;
        this.loadChatUsers();
      })
  }

  public fetchMessages(): void {
    if (this.canView && this.eventId > 0) {
      this.eventChatClient.getListAsync(this.eventId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (r: BaseCollection<EventChatMessage>) =>  {
            this.messages = r.items;
            this.scrollChatToLastMessage();
          },
          error: () => {},
        });
    }
  }

  public get canSendMessageWithNotify(): boolean {
    return this.event?.owner?.id == this.currentUserId;
  }

  public get members(): IUser[] {
    return (this.event) ?
      Event.getMembers(this.event).filter((user: IUser) => user.id !== this.currentUserId)
      : [];
  }

  public get canSendMessage(): boolean {
    return this.eventId !== undefined && this.eventId > 0 && this.form.valid;
  }

  public sendMessage(): void {
    if (!this.canSendMessage) return;

    const message = this.form.controls['message'].value;
    const notify = this.canSendMessageWithNotify ? this.form.controls['notify'].value : false;
    const option = notify ? ChatMessageOption.WithNotify : ChatMessageOption.Default;
    const chatMessage = new EventChatMessage(
      this.eventId,
      this.currentUserId,
      message,
      option,
    );
    this.eventChatClient.sendAsync(chatMessage)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.form.controls['message'].reset();
          this.form.controls['notify'].setValue(false);
        },
        error: () => this.errorProcessor.Process('При отправке произошла ошибка. Пожалуйста, повторите'),
      });
  }
}
