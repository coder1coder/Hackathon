import {AfterViewInit, Component, Injectable, Input, OnInit} from '@angular/core';
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

@Component({
  selector: 'chat-event',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss']
})

@Injectable()
export class ChatEventComponent extends BaseChatComponent<EventChatMessage> implements OnInit, AfterViewInit {

  private _canView: boolean;

  public event: Event | undefined;

  public chatHeaderText = 'Чат мероприятия';

  @Input("eventId")
  set eventId(value) { this._eventId.next(value); };
  get eventId() { return this._eventId.getValue(); }
  private _eventId = new BehaviorSubject<number>(0);

  @Input()
  showMembers: boolean = false;

  form:FormGroup;

  constructor(
    authService: AuthService,
    private signalRService: SignalRService,
    private eventClient: EventClient,
    private eventChatClient: EventChatClient
    ) {
    super(authService)
    signalRService.onChatMessageChanged = (_=> this.fetchMessages());
  }

  ngOnInit(): void {

    this.initForm();

    this._eventId.subscribe(value=>{

      if (value < 1)
        return;

      this.fetchEvent();
      this.fetchMessages();
    })
  }

  fetchEvent(){
    this.eventClient.getById(this.eventId)
    .subscribe(x=>{
      this.event = x;
    })
  }

  get canView(): boolean {
    return this._canView;
  }

  fetchMessages(): void {
    if (this._canView && this.eventId > 0)
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

  updateChatView(){
    this._canView = false;
    if (this.authService.isLoggedIn())
    {
      this.currentUserId = this.authService.getUserId() ?? 0;
      this._canView = this.eventId != null;

      this.fetchMessages();
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

  private initForm():void{
    this.form = new FormGroup({
      message: new FormControl('',[
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(200)
      ]),
      notify: new FormControl(false, [
        Validators.required
      ]),
    })
  }
}
