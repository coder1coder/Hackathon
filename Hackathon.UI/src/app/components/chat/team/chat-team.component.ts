import { Component, ElementRef, Injectable, Input, ViewChild } from '@angular/core';
import { FormBuilder } from "@angular/forms";
import { AuthService } from "../../../services/auth.service";
import { BaseCollection } from "../../../models/BaseCollection";
import { TeamClient } from "../../../services/team-client.service";
import { ChatMessageOption, TeamChatMessage } from "../../../models/chat/TeamChatMessage";
import { BaseChatComponent } from "../base.chat.component";
import { SignalRService } from "../../../services/signalr.service";
import { TeamChatClient } from "../../../clients/chat/team-chat.client";
import { BehaviorSubject, Observable, of, takeUntil } from "rxjs";
import { Team } from "../../../models/Team/Team";
import { IUser } from "../../../models/User/IUser";
import { ITeamChatNewMessageIntegrationEvent } from 'src/app/models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent';
import { ProfileUserStore } from "../../../shared/stores/profile-user.store";
import { ErrorProcessorService } from "../../../services/error-processor.service";

@Component({
  selector: 'chat-team',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss'],
})

@Injectable()
export class ChatTeamComponent extends BaseChatComponent<TeamChatMessage> {
  @ViewChild('scrollMe') chatBody: ElementRef;

  @Input() team: Team;
  @Input() showMembers: boolean = false;
  @Input() set pageIndex(value: number) { this.selectedPageIndex.next(value) };
  @Input("teamId")
  public set teamId(value) { this.entityId.next(value); };
  public get teamId() { return this.entityId.getValue(); }

  public entityId = new BehaviorSubject<number>(0);
  public messages: TeamChatMessage[] = [];

  constructor(
    protected authService: AuthService,
    protected fb: FormBuilder,
    protected profileUserStore: ProfileUserStore,
    private signalRService: SignalRService,
    private teamService: TeamClient,
    private teamChatClient: TeamChatClient,
    private errorProcessor: ErrorProcessorService,
  ) {
    super(authService, fb, profileUserStore)
    signalRService.onTeamChatNewMessage = (x => this.handleNewMessageEvent(x));
  }

  public handleNewMessageEvent(teamChatNewMessage: ITeamChatNewMessageIntegrationEvent): void {
    if (super.canView && this.teamId > 0 && this.teamId == teamChatNewMessage.teamId) {
      this.teamChatClient.getAsync(teamChatNewMessage.messageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (res: TeamChatMessage) =>  {
            this.messages.unshift(res);
            this.onElementsChanged(this.isUserNearBottom(), res.ownerId === this.currentUserId);
          },
          error: () => {},
        });
    }
  }

  public fetchEntity(needReload: boolean = false): void {
    const request: Observable<Team> = Boolean(this.team) && !needReload ?
      of(this.team) :
      this.teamService.getById(this.teamId);

    request
      .pipe(takeUntil(this.destroy$))
      .subscribe((team: Team) => {
        this.team = team;
        this.loadChatUsers();
        this.scrollChatToLastMessage();
      });
  }

  public fetchMessages(): void {
    if (this.canView && this.teamId > 0) {
      this.teamChatClient.getListAsync(this.teamId, this.params.Offset, this.params.Limit)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (r: BaseCollection<TeamChatMessage>) => {
            this.messages.push(...r.items);
            this.changeParamsAfterLoading(r.totalCount);
          },
          error: () => {},
        });
    }
  }

  public get canSendMessageWithNotify(): boolean {
    return this.team?.owner?.id == this.currentUserId;
  }

  public get members(): IUser[] {
    return (this.team) ?
      this.team.members.filter((user: IUser)=> user.id !== this.currentUserId)
      : [];
  }

  public get canSendMessage(): boolean {
    return this.teamId !== undefined && this.teamId > 0 && this.form.valid;
  }

  public sendMessage(): void {
    if (!this.canSendMessage) return;

    const message = this.form.controls['message'].value;
    const notifyTeam = this.canSendMessageWithNotify ? this.form.controls['notify'].value : false;
    const option = notifyTeam ? ChatMessageOption.WithNotify : ChatMessageOption.Default;
    const chatMessage = new TeamChatMessage(
      this.teamId,
      this.currentUserId,
      message,
      option,
    );
    this.teamChatClient.sendAsync(chatMessage)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.form.controls['message'].reset(),
        error: () => this.errorProcessor.Process('При отправке произошла ошибка. Пожалуйста, повторите'),
      });
  }
}
