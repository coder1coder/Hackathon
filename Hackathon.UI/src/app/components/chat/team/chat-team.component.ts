import {AfterViewInit, Component, Injectable, Input, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
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
import { ITeamChatNewMessageIntegrationEvent } from 'src/app/models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent';

@Component({
  selector: 'chat-team',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss']
})

@Injectable()
export class ChatTeamComponent extends BaseChatComponent<TeamChatMessage> implements OnInit, AfterViewInit {

  private _canView: boolean;
  public team: Team | undefined;
  public chatHeaderText = 'Чат команды';

  @Input("teamId")
  set teamId(value) { this._teamId.next(value); };
  get teamId() { return this._teamId.getValue(); }
  private _teamId = new BehaviorSubject<number>(0);

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
    signalRService.onTeamChatNewMessage = (x=> this.handleNewMessageEvent(x));
  }

  ngOnInit(): void {

    this.initForm();

    this._teamId.subscribe(value=>{

      if (value < 1)
        return;

      this.fetchTeam();
      this.fetchMessages();
    })
  }

  fetchTeam(){
    this.teamService.getById(this.teamId)
    .subscribe(x=>{
      this.team = x;
    })
  }

  get canView(): boolean {
    return this._canView;
  }

  handleNewMessageEvent(x:ITeamChatNewMessageIntegrationEvent){

    if (this._canView && this.teamId > 0 && this.teamId == x.teamId)
    {
      this.teamChatClient.getAsync(x.messageId)
        .subscribe({
          next: (r: TeamChatMessage) =>  {
            this.messages.push(r);
            this.scrollChatToLastMessage();
          },
          error: () => {}
        });
    }
  }

  fetchMessages(): void {
    if (this._canView && this.teamId > 0)
    {
      this.teamChatClient.getListAsync(this.teamId)
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
      this._canView = this.teamId != null;

      this.fetchMessages();
    }
  }

  get canSendMessageWithNotify():boolean{
    return this.team?.owner?.id == this.currentUserId;
  }

  get members():IUser[]{
    return (this.team) ? this.team.members.filter(x=>x.id != this.currentUserId) : [];
  }

  get canSendMessage():boolean{
    return this.teamId !== undefined && this.teamId > 0 && this.form.valid;
  }

  sendMessage():void{

    if (!this.canSendMessage)
      return

    let message = this.form.controls['message'].value;
    let notifyTeam = this.canSendMessageWithNotify ? this.form.controls['notify'].value : false;

    let chatMessage = new TeamChatMessage(this.teamId, this.currentUserId, message,
      notifyTeam ? ChatMessageOption.WithNotify : ChatMessageOption.Default);

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
      notify: new FormControl(false, [
        Validators.required
      ]),
    })
  }
}
