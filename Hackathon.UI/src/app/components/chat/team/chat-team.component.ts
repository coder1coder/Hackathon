import {AfterViewInit, Component, ElementRef, Injectable, Input, OnInit, ViewChild} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from "moment/moment";
import {AuthService} from "../../../services/auth.service";
import {ChatService} from "../../../services/chat/chat.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {BehaviorSubject} from "rxjs";
import {TeamService} from "../../../services/team.service";

@Component({
  selector: 'chat-team',
  templateUrl: './chat-team.component.html',
  styleUrls: ['./chat-team.component.scss']
})

@Injectable()
export class ChatTeamComponent implements OnInit, AfterViewInit {

  isCanView:boolean = false
  currentUserId:number = -1;
  isOpened = false

  isFloatMode = false;

  teamOwnerId: number | undefined;

  public chatHeaderText = 'Чат команды';

  @Input()
  set teamId(value) { this._teamId.next(value); };
  get teamId() { return this._teamId.getValue(); }
  private _teamId = new BehaviorSubject<number>(0);

  @ViewChild('scrollMe') private chatBody: ElementRef | undefined;

  messages:ChatMessage[] = []

  form:FormGroup = new FormGroup({
    message: new FormControl('',[
      Validators.required,
      Validators.minLength(1),
      Validators.maxLength(200)
    ]),
    notifyTeam: new FormControl(false, [
      Validators.required
    ]),
  })

  constructor(
    private authService: AuthService,
    private chatService: ChatService,
    private teamService: TeamService) {

    this.currentUserId = this.authService.getUserId() ?? -1;

    authService.authChange.subscribe(_ => this.updateChatView())
    chatService.onPublished = (_=> this.fetch());
  }

  ngOnInit(): void {
    this._teamId.subscribe(_=>{
      this.fetchTeam();
    })
  }

  fetchTeam(){
    this.teamService.getById(this.teamId)
    .subscribe(x=>{
      this.teamOwnerId = x.owner?.id;
    })
  }

  ngAfterViewInit(): void {
    this.updateChatView()
  }

  updateChatView(){
    this.isCanView = false;
    if (this.authService.isLoggedIn())
    {
      this.currentUserId = this.authService.getUserId() ?? 0;
      this.isCanView = this.teamId != null;

      this.fetch();
    }
  }

  fetch(){
    if (this.isCanView)
    {
      this.chatService.getTeamMessages(this.teamId!)
        .subscribe({
          next: (r: BaseCollection<ChatMessage>) =>  {
            this.messages = r.items
            this.scrollChatToLastMessage();
          },
          error: () => {}
        });
    }
  }

  get canSendMessageWithNotify(){
    return this.teamOwnerId == this.currentUserId;
  }

  sendMessage(){

    if (this.teamId == null)
      return

    if (!this.form.valid)
      return;

    let message = this.form.controls['message'].value;
    let notifyTeam = this.canSendMessageWithNotify ? this.form.controls['notifyTeam'].value : false;

    let chatMessage = new ChatMessage(ChatMessageContext.TeamChat, this.currentUserId, message);
    chatMessage.teamId = this.teamId;
    chatMessage.timestamp = moment.utc().toISOString();
    chatMessage.options = notifyTeam ? ChatMessageOption.WithNotify : ChatMessageOption.Default;

    this.chatService.sendTeamMessage(chatMessage)
      .subscribe(_ => {
        this.form.reset()
      })
  }

  scrollChatToLastMessage(){
      setTimeout(()=>{
        if (this.chatBody !== undefined) {
          this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
        }
      },100)
  }

}

export class ChatMessage{
  ownerId!:number;
  ownerFullName!:string;
  teamId?:number
  userId?:number;
  userFullName!:string;
  message!:string;
  context!:ChatMessageContext;
  options!:ChatMessageOption;
  timestamp!:string;

  constructor(context:ChatMessageContext, ownerId:number, message:string) {
    this.context = context;
    this.ownerId = ownerId;
    this.message = message;
  }
}

export enum ChatMessageContext
{
  TeamChat
}

export enum ChatMessageOption {
  Default = 0,
  WithNotify = 1
}
