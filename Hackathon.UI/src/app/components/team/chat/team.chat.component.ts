import {AfterViewInit, Component, ElementRef, Injectable, OnInit, ViewChild} from '@angular/core';
import {AuthService} from "../../../services/auth.service";
import {FormControl, FormGroup} from "@angular/forms";
import * as moment from "moment/moment";
import {ChatService} from "../../../services/chat/chat.service";
import {TeamService} from "../../../services/team.service";
import {TeamModel} from "../../../models/Team/TeamModel";
import {SnackService} from "../../../services/snack.service";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";

@Component({
  selector: 'team-chat',
  templateUrl: './team.chat.component.html',
  styleUrls: ['./team.chat.component.scss']
})

@Injectable()
export class TeamChatComponent implements AfterViewInit {

  isCanView:boolean = false
  currentUserId:number = -1;
  userTeam:TeamModel | null = null
  isOpened = false

  @ViewChild('scrollMe') private chatBody: ElementRef | undefined;

  messages:ChatMessage[] = []

  form:FormGroup = new FormGroup({
    message: new FormControl('')
  })

  constructor(
    private authService: AuthService,
    private chatService:ChatService,
    private teamService:TeamService,
    private snack:SnackService) {

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
      this.teamService.getMyTeam().subscribe( r => {
        this.userTeam = r;
        this.isCanView = this.userTeam != null;
        this.fetch()
      });
    }
  }

  fetch(){
    if (this.isCanView)
    {

      this.currentUserId = this.authService.getUserId() ?? 0;

      if (this.currentUserId >= 0 && this.userTeam != null)
      {
          this.chatService.getTeamMessages(this.userTeam.id)
            .subscribe({
              next: (r: BaseCollectionModel<ChatMessage>) =>  {
                this.messages = r.items
                this.scrollChatToLastMessage();
              },
              error: () => {}
            });
      }
    }
  }

  sendMessage(){
    //TODO: more validation
    if (this.userTeam == null)
    {
      this.snack.open("Вы не состоите в команде, чтобы отправить сообщение");
      return
    }

    let message = this.form.controls['message'].value;
    let ownerId = this.authService.getUserId() ?? -1;

    let chatMessage = new ChatMessage(ChatMessageContext.TeamChat, ownerId, message);
    chatMessage.teamId = this.userTeam?.id;
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

