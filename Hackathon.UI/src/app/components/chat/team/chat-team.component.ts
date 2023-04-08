import {AfterViewInit, Component, Injectable, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import * as moment from "moment/moment";
import {AuthService} from "../../../services/auth.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {TeamClient} from "../../../services/team-client.service";
import {ChatMessageOption, TeamChatMessage} from "../../../models/chat/TeamChatMessage";
import {BaseChatComponent} from "../base.chat.component";
import {SignalRService} from "../../../services/signalr.service";
import {TeamChatClient} from "../../../clients/chat/team-chat.client";
import {BehaviorSubject} from "rxjs";
import {Team} from "../../../models/Team/Team";
import {IUser} from "../../../models/User/IUser";

@Component({
  selector: 'chat-team',
  templateUrl: './chat-team.component.html',
  styleUrls: ['./chat-team.component.scss']
})

@Injectable()
export class ChatTeamComponent extends BaseChatComponent<TeamChatMessage> implements OnInit, AfterViewInit {

  private _canView: boolean;


  public team: Team | undefined;

  public chatHeaderText = 'Чат команды';

  @Input("chatId")
  set chatId(value) { this._chatId.next(value); };
  get chatId() { return this._chatId.getValue(); }
  private _chatId = new BehaviorSubject<number>(0);

  @Input()
  showMembers: boolean = false;

  form:FormGroup;

  constructor(
    authService: AuthService,
    private signalRService: SignalRService,
    private teamService: TeamClient,
    private teamChatClient: TeamChatClient
    ) {
    super(authService)
    signalRService.onChatMessageChanged = (_=> this.fetchMessages());
  }

  ngOnInit(): void {

    this.initForm();

    this._chatId.subscribe(value=>{

      if (value < 1)
        return;

      this.fetchTeam();
      this.fetchMessages();
    })
  }

  fetchTeam(){
    this.teamService.getById(this.chatId)
    .subscribe(x=>{
      this.team = x;
    })
  }

  get canView(): boolean {
    return this._canView;
  }

  fetchMessages(): void {
    if (this._canView && this.chatId > 0)
    {
      this.teamChatClient.getListAsync(this.chatId)
        .subscribe({
          next: (r: BaseCollection<TeamChatMessage>) =>  {
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
      this._canView = this.chatId != null;

      this.fetchMessages();
    }
  }

  get canSendMessageWithNotify():boolean{
    return this.team?.owner?.id == this.currentUserId;
  }

  get members():IUser[]{
    return (this.team) ? this.team.members : [];
  }

  get canSendMessage():boolean{
    return this.chatId !== undefined && this.chatId > 0 && this.form.valid;
  }

  sendMessage():void{

    if (!this.canSendMessage)
      return

    let message = this.form.controls['message'].value;
    let notifyTeam = this.canSendMessageWithNotify ? this.form.controls['notifyTeam'].value : false;

    let chatMessage = new TeamChatMessage(this.currentUserId, message);
    chatMessage.teamId = this.chatId;
    chatMessage.timestamp = moment.utc().toISOString();
    chatMessage.options = notifyTeam ? ChatMessageOption.WithNotify : ChatMessageOption.Default;

    this.teamChatClient.sendAsync(chatMessage)
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
      notifyTeam: new FormControl(false, [
        Validators.required
      ]),
    })
  }
}
