import {Component, Input} from "@angular/core";
import {Team} from "../../../models/Team/Team";
import {RouterService} from "../../../services/router.service";
import {EventHttpService} from "../../../services/event/event.http-service";
import {EventFilter} from "../../../models/Event/EventFilter";
import {GetListParameters, SortOrder} from "../../../models/GetListParameters";
import {MatTabChangeEvent} from "@angular/material/tabs";
import {IEventListItem} from "../../../models/Event/IEventListItem";
import {AuthService} from "../../../services/auth.service";
import {MatTableDataSource} from "@angular/material/table";
import {
  ITeamJoinRequest,
} from "../../../models/Team/ITeamJoinRequest";
import {TeamClient} from "../../../services/team-client.service";
import {MatDialog} from "@angular/material/dialog";
import {
  CancelJoinRequestCommentDialog
} from "../cancelJoinRequestCommentDialog/cancelJoinRequestCommentDialog.component";
import {ICancelRequestParameters} from "../../../models/Team/CancelRequestParameters";

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})

export class TeamComponent {

  @Input() team: Team;

  public teamEvents: IEventListItem[] = [];
  public authorizedUserId = this.authService.getUserId();
  public sentTeamJoinRequestsDataSource: MatTableDataSource<ITeamJoinRequest> = new MatTableDataSource<ITeamJoinRequest>([]);

  constructor(
    public dialog: MatDialog,
    public router: RouterService,
    private eventHttpService: EventHttpService,
    private authService: AuthService,
    private teamClient: TeamClient
  ) {
  }

  public get isTeamMember(): boolean {
    if (!Boolean(this.team)) return false;
    if (this.team.owner?.id === this.authorizedUserId) return true;
    return this.team?.members.some(member => member.id === this.authorizedUserId);
  }

  public tabChanged(event: MatTabChangeEvent): void {

    if (!this.team)
      return

    if (event.index == 2) {
      let getList = new GetListParameters<EventFilter>();
      getList.Filter = new EventFilter();
      getList.Filter.teamsIds = [ this.team.id ];

      this.eventHttpService.getList(getList)
        .subscribe(x => this.teamEvents = x.items);
    }

    if (event.index == 3 && this.team.owner?.id == this.authorizedUserId)
    {
      this.fetchTeamSentJoinRequests();
    }
  }

  private fetchTeamSentJoinRequests() {

    this.teamClient.getTeamSentJoinRequests(this.team.id, {
      Offset: 0,
      Limit: 100,
      SortBy: 'createdAt',
      SortOrder: SortOrder.Asc
    })
      .subscribe({
        next: (res) => {
          this.sentTeamJoinRequestsDataSource.data = res?.items;
        },
        error: () => {
        }
      });
  }

  cancelJoinRequestByOwner(requestId:number) {

    const dialogRef = this.dialog.open(CancelJoinRequestCommentDialog, {
      data: null,
      minWidth: 320
    });

    let parameters: ICancelRequestParameters = {
      requestId: requestId,
      comment: null
    };

    dialogRef.afterClosed().subscribe(result => {
      parameters.comment = result;
      this.teamClient.cancelJoinRequest(parameters).subscribe(_=>{
        this.fetchTeamSentJoinRequests()
      })
    });

  }
}
