import { Component, OnDestroy, OnInit } from "@angular/core";
import { RouterService } from "../../../services/router.service";
import { TeamClient } from "../../../services/team-client.service";
import { Team } from "../../../models/Team/Team";
import { AuthService } from "../../../services/auth.service";
import { Router } from "@angular/router";
import { Subject, takeUntil } from "rxjs";
import { ITeamJoinRequest, TeamJoinRequestStatus, TeamJoinRequestStatusTranslator } from "../../../models/Team/ITeamJoinRequest";
import { GetListParameters, SortOrder } from "../../../models/GetListParameters";
import { ITeamJoinRequestFilter } from "../../../models/Team/ITeamJoinRequestFilter";
import { MatTableDataSource } from "@angular/material/table";
import { BaseCollection } from "../../../models/BaseCollection";
import { ErrorProcessorService } from "../../../services/error-processor.service";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss'],
})
export class UserTeamComponent implements OnInit, OnDestroy {
  public team: Team;
  public sentTeamJoinRequestsDataSource: MatTableDataSource<ITeamJoinRequest> = new MatTableDataSource<ITeamJoinRequest>([]);
  public TeamJoinRequestStatusTranslator = TeamJoinRequestStatusTranslator;

  private destroy$ = new Subject();

  constructor(
    public routerService: RouterService,
    private teamClient: TeamClient,
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

  public leaveTeam(): void  {
    if (this.team?.id !== undefined) {
      this.teamClient.leaveTeam(this.team.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.navigate([this.router.url]);
        });
    }
  }

  public cancelJoinRequest(requestId: number): void {
    this.teamClient.cancelJoinRequest({
      requestId: requestId,
      comment: null,
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchSentJoinRequests(),
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  private fetchTeam(): void {
    this.teamClient.getMyTeam()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: Team) => this.team = res,
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  private fetchSentJoinRequests(): void {
    const parameters: GetListParameters<ITeamJoinRequestFilter> = {
      Filter: {
        status: TeamJoinRequestStatus.Sent
      },
      Offset: 0,
      Limit: 5,
      SortBy: 'createdAt',
      SortOrder: SortOrder.Desc
    };

    this.teamClient.getJoinRequests(parameters)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<ITeamJoinRequest>) => this.sentTeamJoinRequestsDataSource.data = res?.items,
        error: (error) => this.errorProcessor.Process(error),
      });
  }
}
