import {AfterViewInit, Component, ElementRef, Injectable, Input, ViewChild} from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";
import * as moment from "moment/moment";
import {AuthService} from "../../../services/auth.service";
import {ChatService} from "../../../services/chat/chat.service";
import {BaseCollection} from "../../../models/BaseCollection";

@Component({
  selector: 'chat-team',
  templateUrl: './chat-team.component.html',
  styleUrls: ['./chat-team.component.scss']
})

@Injectable()
export class ChatTeamComponent implements AfterViewInit {

  isCanView:boolean = false
  currentUserId:number = -1;
  isOpened = false

  isFloatMode = false;

  public chatHeaderText = 'Чат команды';

  @Input()
  public teamId?:number;

  @ViewChild('scrollMe') private chatBody: ElementRef | undefined;

  messages:ChatMessage[] = []

  form:FormGroup = new FormGroup({
    message: new FormControl('')
  })

  constructor(
    private authService: AuthService,
    private chatService:ChatService) {

    authService.authChange.subscribe(_ => this.updateChatView())
    chatService.onPublished = (_=> this.fetch());
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

  sendMessage(){
    //TODO: more validation
    if (this.teamId == null)
      return

    let message = this.form.controls['message'].value;
    let ownerId = this.authService.getUserId() ?? -1;

    let chatMessage = new ChatMessage(ChatMessageContext.TeamChat, ownerId, message);
    chatMessage.teamId = this.teamId;
    chatMessage.timestamp = moment.utc().toISOString();

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
  teamId!:number
  userId?:number;
  userFullName!:string;
  message!:string;
  context!:ChatMessageContext;
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

