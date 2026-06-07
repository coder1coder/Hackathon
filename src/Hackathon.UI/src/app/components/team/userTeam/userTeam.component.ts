import { Component, OnDestroy, OnInit } from '@angular/core';
import { RouterService } from '../../../services/router.service';
import { Team } from '../../../models/Team/Team';
import { AuthService } from '../../../services/auth.service';
import { Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import {
  ITeamJoinRequest,
  TeamJoinRequestStatus,
  TeamJoinRequestStatusTranslator,
} from '../../../models/Team/ITeamJoinRequest';
import { GetListParameters, SortOrder } from '../../../models/GetListParameters';
import { ITeamJoinRequestFilter } from '../../../models/Team/ITeamJoinRequestFilter';
import { MatTableDataSource, MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow } from '@angular/material/table';
import { BaseCollection } from '../../../models/BaseCollection';
import { ErrorProcessorService } from '../../../services/error-processor.service';
import { TeamsClient } from 'src/app/clients/teams.client';
import { HttpErrorResponse } from '@angular/common/http';
import { DefaultLayoutComponent } from '../../layouts/default/default.layout.component';
import { NgIf } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { TeamComponent } from '../team/team.component';
import { AlertComponent } from '../../custom/alert/alert.component';

@Component({
    selector: 'userTeam',
    templateUrl: 'userTeam.component.html',
    styleUrls: ['userTeam.component.scss'],
    imports: [DefaultLayoutComponent, NgIf, MatButton, MatIcon, TeamComponent, AlertComponent, MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow]
})
export class UserTeamComponent implements OnInit, OnDestroy {
  public team!: Team;
  public sentTeamJoinRequestsDataSource: MatTableDataSource<ITeamJoinRequest> =
    new MatTableDataSource<ITeamJoinRequest>([]);
  public TeamJoinRequestStatusTranslator = TeamJoinRequestStatusTranslator;

  private destroy$ = new Subject();

  constructor(
    public routerService: RouterService,
    private teamsClient: TeamsClient,
    private authService: AuthService,
    private router: Router,
    private errorProcessor: ErrorProcessorService,
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) return;
    this.fetchTeam();
    this.fetchSentJoinRequests();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public leaveTeam(): void {
    if (this.team?.id !== undefined) {
      this.teamsClient
        .leaveTeam(this.team.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.router.routeReuseStrategy.shouldReuseRoute = (): boolean => false;
          this.router.navigate([this.router.url]);
        });
    }
  }

  public cancelJoinRequest(requestId: number): void {
    this.teamsClient
      .cancelJoinRequest({
        requestId: requestId,
        comment: '',
      })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchSentJoinRequests(),
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  private fetchTeam(): void {
    this.teamsClient
      .getMyTeam()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: Team) => (this.team = res),
        error: (error: HttpErrorResponse) => {
          if (error.status !== 404) {
            /* нет необходимости отображать пользователю сообщение об ошибке если у пользователя нет команды */
            this.errorProcessor.Process(error);
          }
        },
      });
  }

  private fetchSentJoinRequests(): void {
    const parameters: GetListParameters<ITeamJoinRequestFilter> = {
      Filter: {
        status: TeamJoinRequestStatus.Sent,
      },
      Offset: 0,
      Limit: 5,
      SortBy: 'createdAt',
      SortOrder: SortOrder.Desc,
    };

    this.teamsClient
      .getJoinRequests(parameters)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<ITeamJoinRequest>) =>
          (this.sentTeamJoinRequestsDataSource.data = res?.items),
        error: (error) => this.errorProcessor.Process(error),
      });
  }
}
