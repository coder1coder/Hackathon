import {Component, OnInit} from "@angular/core";
import {RouterService} from "../../../services/router.service";
import {TeamClient} from "../../../services/team-client.service";
import {Team} from "../../../models/Team/Team";
import {AuthService} from "../../../services/auth.service";
import {Router} from "@angular/router";
import {finalize} from "rxjs/operators";
import {Subject, takeUntil} from "rxjs";
import {ITeamJoinRequest, TeamJoinRequestStatus, TeamJoinRequestStatusTranslator} from "../../../models/Team/ITeamJoinRequest";
import {GetListParameters, SortOrder} from "../../../models/GetListParameters";
import {ITeamJoinRequestFilter} from "../../../models/Team/ITeamJoinRequestFilter";
import {MatTableDataSource} from "@angular/material/table";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss']
})

export class UserTeamComponent implements OnInit {
  public team: Team;
  public isLoading: boolean = true;

  private destroy$ = new Subject();

  public sentTeamJoinRequestsDataSource: MatTableDataSource<ITeamJoinRequest> = new MatTableDataSource<ITeamJoinRequest>([]);
  public TeamJoinRequestStatusTranslator = TeamJoinRequestStatusTranslator;

  constructor(
    public routerService: RouterService,
    private teamClient: TeamClient,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) return;

    this.fetchTeam();
    this.fetchSentJoinRequests();
  }

  public leaveTeam(): void  {
    if (this.team !== undefined) {
      this.teamClient.leaveTeam(this.team?.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.navigate([this.router.url]);
        });
    }
  }

  private fetchTeam() {
    this.isLoading = true;

    this.teamClient.getMyTeam()
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isLoading = false),
      )
      .subscribe({
        next: (res) => {
          this.team = res;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  private fetchSentJoinRequests() {
    this.isLoading = true;

    let parameters: GetListParameters<ITeamJoinRequestFilter> = {
      Filter: {
        status: TeamJoinRequestStatus.Sent
      },
      Offset: 0,
      Limit: 5,
      SortBy: 'createdAt',
      SortOrder: SortOrder.Desc
    };

    this.teamClient.getJoinRequests(parameters)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isLoading = false),
      )
      .subscribe({
        next: (res) => {

          console.log(res.items)
          this.sentTeamJoinRequestsDataSource.data = res?.items;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  cancelJoinRequest(teamId: number) {
    this.teamClient.cancelJoinRequest(teamId)
      .subscribe({
        next: (res) => {
          this.fetchSentJoinRequests();
        },
        error: () => {
        }
      });
  }
}
