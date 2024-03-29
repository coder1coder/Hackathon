import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { BaseCollection } from '../../../models/BaseCollection';
import { ChatMessageOption, TeamChatMessage } from '../../../models/chat/TeamChatMessage';
import { BaseChatComponent } from '../base.chat.component';
import { SignalRService } from '../../../services/signalr.service';
import { BehaviorSubject, Observable, of, takeUntil } from 'rxjs';
import { Team } from '../../../models/Team/Team';
import { IUser } from '../../../models/User/IUser';
import { ITeamChatNewMessageIntegrationEvent } from 'src/app/models/chat/integrationEvents/ITeamChatNewMessageIntegrationEvent';
import { ProfileUserStore } from '../../../shared/stores/profile-user.store';
import { ErrorProcessorService } from '../../../services/error-processor.service';
import { TeamsClient } from 'src/app/clients/teams.client';
import { TeamChatsClient } from 'src/app/clients/team-chats.client';

@Component({
  selector: 'chat-team',
  templateUrl: '../base.chat.component.html',
  styleUrls: ['../base.chat.component.scss'],
})
export class ChatTeamComponent extends BaseChatComponent<TeamChatMessage> implements OnInit {
  @ViewChild('scrollMe') chatBody: ElementRef;

  @Input() team: Team;
  @Input() showMembers: boolean = false;
  @Input() set pageIndex(value: number) {
    this.selectedPageIndex.next(value);
  }
  @Input()
  public set teamId(value) {
    this.entityId.next(value);
  }
  public get teamId(): number {
    return this.entityId.getValue();
  }

  public entityId = new BehaviorSubject<number>(0);
  public messages: TeamChatMessage[] = [];

  constructor(
    protected authService: AuthService,
    protected fb: FormBuilder,
    protected profileUserStore: ProfileUserStore,
    private signalRService: SignalRService,
    private teamsClient: TeamsClient,
    private teamChatsClient: TeamChatsClient,
    private errorProcessor: ErrorProcessorService,
  ) {
    super(authService, fb, profileUserStore);
    signalRService.onTeamChatNewMessage = (x): void => this.handleNewMessageEvent(x);
  }

  ngOnInit(): void {
    this.initChat();
  }

  public handleNewMessageEvent(teamChatNewMessage: ITeamChatNewMessageIntegrationEvent): void {
    if (super.canView && this.teamId > 0 && this.teamId == teamChatNewMessage.teamId) {
      this.teamChatsClient
        .getAsync(teamChatNewMessage.messageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (res: TeamChatMessage) => {
            this.messages.unshift(res);
            this.onElementsChanged(this.isUserNearBottom, res.ownerId === this.currentUserId);
          },
          error: () => {},
        });
    }
  }

  public fetchEntity(needReload: boolean = false): void {
    const request: Observable<Team> =
      Boolean(this.team) && !needReload ? of(this.team) : this.teamsClient.getById(this.teamId);

    request.pipe(takeUntil(this.destroy$)).subscribe((team: Team) => {
      this.team = team;
      this.loadChatUsers();
      this.scrollChatToLastMessage();
    });
  }

  public fetchMessages(): void {
    if (this.canView && this.teamId > 0) {
      this.teamChatsClient
        .getListAsync(this.teamId, this.params.Offset, this.params.Limit)
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
    return this.team
      ? this.team.members.filter((user: IUser) => user.id !== this.currentUserId)
      : [];
  }

  public get canSendMessage(): boolean {
    return this.teamId !== undefined && this.teamId > 0 && this.form.valid;
  }

  public sendMessage(): void {
    if (!this.canSendMessage) return;

    const message: string = this.form.controls['message'].value;
    const notifyTeam: boolean = this.canSendMessageWithNotify
      ? this.form.controls['notify'].value
      : false;
    const option: ChatMessageOption = notifyTeam
      ? ChatMessageOption.WithNotify
      : ChatMessageOption.Default;
    const chatMessage: TeamChatMessage = new TeamChatMessage(
      this.teamId,
      this.currentUserId,
      message,
      option,
    );
    this.teamChatsClient
      .sendAsync(chatMessage)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.form.controls['message'].reset(),
        error: () =>
          this.errorProcessor.Process('При отправке произошла ошибка. Пожалуйста, повторите'),
      });
  }
}
