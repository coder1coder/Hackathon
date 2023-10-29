import {AfterViewInit, Component, ElementRef, Injectable, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../../services/auth.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {ChatMessageOption} from "../../../models/chat/TeamChatMessage";
import {BaseChatComponent} from "../base.chat.component";
import {SignalRService} from "../../../services/signalr.service";
import {BehaviorSubject} from "rxjs";
import {IUser} from "../../../models/User/IUser";
import {EventChatMessage} from "../../../models/chat/EventChatMessage";
import {EventChatClient} from "../../../clients/chat/event-chat.client";
import {Event} from "../../../models/Event/Event";
import {EventClient} from "../../../services/event/event.client";
import { IEventChatNewMessageIntegrationEvent } from 'src/app/models/chat/integrationEvents/IEventChatNewMessageIntegrationEvent';

@Component({
  selector: 'chat-event',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss']
})

@Injectable()
export class ChatEventComponent extends BaseChatComponent<EventChatMessage> {

  public event: Event | undefined;

  public chatHeaderText = 'Чат мероприятия';

  @ViewChild('scrollMe') chatBody: ElementRef | undefined;

  @Input("eventId")
  set eventId(value) { this.entityId.next(value); };
  get eventId() { return this.entityId.getValue(); }
  entityId = new BehaviorSubject<number>(0);
  public messages:EventChatMessage[] = [];

  @Input()
  showMembers: boolean = false;

  constructor(
    authService: AuthService,
    private signalRService: SignalRService,
    private eventClient: EventClient,
    private eventChatClient: EventChatClient
    ) {
    super(authService)
    signalRService.onEventChatNewMessage = (x =>
      this.handleNewMessageEvent(x));
  }

  handleNewMessageEvent(x:IEventChatNewMessageIntegrationEvent){

    if (this.canView && this.eventId > 0 && this.eventId == x.eventId)
    {
      this.eventChatClient.getAsync(x.messageId)
        .subscribe({
          next: (r: EventChatMessage) =>  {
            this.messages.push(r);
            this.scrollChatToLastMessage();
          },
          error: () => {}
        });
    }
  }

  fetchEntity(){
    this.eventClient.getById(this.eventId)
    .subscribe(x=>{
      this.event = x;
    })
  }

  fetchMessages(): void {

    if (this.canView && this.eventId > 0)
    {
      this.eventChatClient.getListAsync(this.eventId)
        .subscribe({
          next: (r: BaseCollection<EventChatMessage>) =>  {
            this.messages = r.items
            this.scrollChatToLastMessage();
          },
          error: () => {}
        });
    }
  }

  get canSendMessageWithNotify():boolean{
    return this.event?.owner?.id == this.currentUserId;
  }

  get members():IUser[]{
    return (this.event) ? Event.getMembers(this.event).filter(x=>x.id != this.currentUserId) : [];
  }

  get canSendMessage():boolean{
    return this.eventId !== undefined && this.eventId > 0 && this.form.valid;
  }

  sendMessage():void{

    if (!this.canSendMessage)
      return

    let message = this.form.controls['message'].value;
    let notify = this.canSendMessageWithNotify ? this.form.controls['notify'].value : false;

    let chatMessage = new EventChatMessage(
      this.eventId,
      this.currentUserId,
      message,
      notify ? ChatMessageOption.WithNotify : ChatMessageOption.Default);

    this.eventChatClient.sendAsync(chatMessage)
      .subscribe(_ => {
        this.initForm();
      })
  }
}
