import {AfterViewInit, Component, ElementRef, Injectable, Input, OnInit, ViewChild} from '@angular/core';
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
export class ChatTeamComponent extends BaseChatComponent<TeamChatMessage> {

  public team: Team | undefined;
  public chatHeaderText = 'Чат команды';

  @Input("teamId")
  set teamId(value) { this.entityId.next(value); };
  get teamId() { return this.entityId.getValue(); }
  entityId = new BehaviorSubject<number>(0);

  public messages:TeamChatMessage[] = [];

  @Input()
  showMembers: boolean = false;

  @ViewChild('scrollMe') chatBody: ElementRef | undefined;

  constructor(
    authService: AuthService,
    private signalRService: SignalRService,
    private teamService: TeamClient,
    private teamChatClient: TeamChatClient
    ) {
    super(authService)
    signalRService.onTeamChatNewMessage = (x=> this.handleNewMessageEvent(x));
  }

  fetchEntity(){
    this.teamService.getById(this.teamId)
    .subscribe(x=>{
      this.team = x;
    })
  }

  handleNewMessageEvent(x:ITeamChatNewMessageIntegrationEvent){

    if (super.canView && this.teamId > 0 && this.teamId == x.teamId)
    {
      this.teamChatClient.getAsync(x.messageId)
        .subscribe({
          next: (r: TeamChatMessage) =>  {
            this.messages.push(r);
          },
          error: () => {}
        });
    }
  }

  fetchMessages(): void {

    if (this.canView && this.teamId > 0)
    {
      this.teamChatClient.getListAsync(this.teamId)
        .subscribe({
          next: (r: BaseCollection<TeamChatMessage>) =>  {
            this.messages = r.items
          },
          error: () => {}
        });
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
}
