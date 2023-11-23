import { Component, Input, OnDestroy } from "@angular/core";
import { Team } from "../../../models/Team/Team";
import { RouterService } from "../../../services/router.service";
import { EventClient } from "../../../services/event/event.client";
import { EventFilter } from "../../../models/Event/EventFilter";
import { GetListParameters, SortOrder } from "../../../models/GetListParameters";
import { MatTabChangeEvent } from "@angular/material/tabs";
import { IEventListItem } from "../../../models/Event/IEventListItem";
import { AuthService } from "../../../services/auth.service";
import { MatTableDataSource } from "@angular/material/table";
import { ITeamJoinRequest } from "../../../models/Team/ITeamJoinRequest";
import { TeamClient } from "../../../services/team-client.service";
import { MatDialog } from "@angular/material/dialog";
import { CancelJoinRequestCommentDialog } from "../cancelJoinRequestCommentDialog/cancelJoinRequestCommentDialog.component";
import { ICancelRequestParameters } from "../../../models/Team/CancelRequestParameters";
import { Subject, switchMap, takeUntil } from "rxjs";
import { BaseCollection } from "../../../models/BaseCollection";

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})

export class TeamComponent implements OnDestroy {
  @Input() team: Team;

  public teamEvents: IEventListItem[] = [];
  public authorizedUserId = this.authService.getUserId();
  public sentTeamJoinRequestsDataSource: MatTableDataSource<ITeamJoinRequest> = new MatTableDataSource<ITeamJoinRequest>([]);
  public tabIndex: number = 0;

  private destroy$ = new Subject();

  constructor(
    public dialog: MatDialog,
    public router: RouterService,
    private eventHttpService: EventClient,
    private authService: AuthService,
    private teamClient: TeamClient,
  ) {
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private fetchTeam(): void {
    this.teamClient.getById(this.team.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe((res: Team) => this.team = res);
  }

  public get isTeamMember(): boolean {
    if (!Boolean(this.team)) return false;
    if (this.team.owner?.id === this.authorizedUserId) return true;
    return this.team?.members.some(member => member.id === this.authorizedUserId);
  }

  public getMemberStatus(memberId:number): string {
    if (!Boolean(this.team)) return '';
    return (this.team.owner?.id === memberId)
      ? 'Владелец'
      : 'Участник';
  }

  public tabChanged(event: MatTabChangeEvent): void {
    if (!this.team) return;
    this.tabIndex = event.index;
    if (event.index === 2) {
      const getList = new GetListParameters<EventFilter>();
      getList.Filter = new EventFilter();
      getList.Filter.teamsIds = [ this.team.id ];

      this.eventHttpService.getList(getList)
        .pipe(takeUntil(this.destroy$))
        .subscribe((res: BaseCollection<IEventListItem>) => this.teamEvents = res.items);
    }

    if (event.index === 3 && this.team.owner?.id === this.authorizedUserId) {
      this.fetchTeamSentJoinRequests();
    }
  }

  public approveJoinRequestByOwner(requestId: number): void {
    this.teamClient.approveJoinRequest(requestId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
      this.fetchTeam();
      this.fetchTeamSentJoinRequests();
    })
  }

  public cancelJoinRequestByOwner(requestId: number): void {
    const dialogRef = this.dialog.open(CancelJoinRequestCommentDialog, {
      data: null,
      minWidth: 320,
    });

    let parameters: ICancelRequestParameters = {
      requestId: requestId,
      comment: null,
    };

    dialogRef.afterClosed()
      .pipe(
        switchMap((result: string) => {
          parameters.comment = result;
          return this.teamClient.cancelJoinRequest(parameters);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe(() => this.fetchTeamSentJoinRequests());

  }

  private fetchTeamSentJoinRequests(): void {
    this.teamClient.getTeamSentJoinRequests(this.team.id, {
      Offset: 0,
      Limit: 100,
      SortBy: 'createdAt',
      SortOrder: SortOrder.Asc,
    })
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<ITeamJoinRequest>) => {
          this.sentTeamJoinRequestsDataSource.data = res?.items;
        },
        error: () => {},
      });
  }
}
