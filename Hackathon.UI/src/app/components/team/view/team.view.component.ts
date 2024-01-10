import { OnInit, Component, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Team, TeamType } from '../../../models/Team/Team';
import { TeamClient } from '../../../services/team-client.service';
import { finalize } from 'rxjs/operators';
import { RouterService } from '../../../services/router.service';
import { AuthService } from '../../../services/auth.service';
import { SnackService } from '../../../services/snack.service';
import { IProblemDetails } from '../../../models/IProblemDetails';
import { ITeamJoinRequest } from '../../../models/Team/ITeamJoinRequest';
import { Subject, takeUntil } from 'rxjs';
import { AppStateService } from '../../../services/app-state.service';

@Component({
  selector: 'team-view',
  templateUrl: './team.view.component.html',
  styleUrls: ['./team.view.component.scss'],
})
export class TeamViewComponent implements OnInit, OnDestroy {
  @Input() teamId?: number;

  public team: Team;
  public userId: number;
  public existsSentJoinRequest: ITeamJoinRequest;

  private destroy$ = new Subject();

  constructor(
    private activateRoute: ActivatedRoute,
    public router: RouterService,
    private teamClient: TeamClient,
    private authService: AuthService,
    private snackService: SnackService,
    private appStateService: AppStateService,
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId() ?? 0;
    if (this.teamId == null) {
      this.teamId = this.activateRoute.snapshot.params['teamId'];
    }
    this.fetch();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public get canJoinToTeam(): boolean {
    return (
      this.team &&
      this.team.type == TeamType.Public &&
      !this.teamContainsMember(this.team, this.userId)
    );
  }

  public get canLeaveTeam(): boolean {
    return this.team && this.teamContainsMember(this.team, this.userId);
  }

  public get canSendRequestJoinToTeam(): boolean {
    return (
      this.team &&
      this.team.type == TeamType.Private &&
      !this.teamContainsMember(this.team, this.userId) &&
      !this.hasSentJoinRequest
    );
  }

  public leaveTeam(): void {
    this.teamClient
      .leaveTeam(this.teamId ?? 0)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.router.Teams.MyTeam(),
        error: (err) => {
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        },
      });
  }

  public joinToTeam(): void {
    this.teamClient
      .joinToTeam(this.teamId ?? 0)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.router.Teams.MyTeam();
        },
        error: (err) => {
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        },
      });
  }

  public sendRequestJoinToTeam(): void {
    if (!this.teamId) {
      this.snackService.open('Не удалось определить идентификатор команды');
      return;
    }

    this.teamClient
      .createJoinRequest(this.teamId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackService.open('Запрос на вступление в команду отправлен');
          this.fetch();
        },
        error: (err) => {
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        },
      });
  }

  public cancelSentJoinRequest(): void {
    if (!this.existsSentJoinRequest) {
      this.snackService.open('Запрос на вступление в команду не найден');
      return;
    }

    this.teamClient
      .cancelJoinRequest({
        requestId: this.existsSentJoinRequest.id,
        comment: null,
      })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snackService.open('Запрос на вступление в команду отменен');
          this.fetch();
        },
        error: (err) => {
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        },
      });
  }

  public get hasSentJoinRequest(): boolean {
    return this.existsSentJoinRequest !== null;
  }

  private fetch(): void {
    this.fetchTeam();
    this.fetchSentJoinRequest();
  }

  private fetchTeam(): void {
    this.appStateService.setIsLoadingState(true);
    this.teamClient
      .getById(this.teamId)
      .pipe(
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res: Team) => (this.team = res),
        error: () => {},
      });
  }

  private fetchSentJoinRequest(): void {
    this.appStateService.setIsLoadingState(true);
    this.teamClient
      .getSentJoinRequest(this.teamId!)
      .pipe(
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res: ITeamJoinRequest) => (this.existsSentJoinRequest = res),
        error: () => (this.existsSentJoinRequest = null),
      });
  }

  private teamContainsMember(team: Team, memberId: number): boolean {
    return (
      (team.owner && team.owner.id == memberId) ||
      (team.members &&
        team.members.length > 0 &&
        team.members.filter((x) => x.id == memberId).length > 0)
    );
  }
}
