import {AfterViewInit, Component, Input} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Team, TeamType} from "../../../models/Team/Team";
import {TeamService} from "../../../services/team.service";
import {finalize} from "rxjs/operators";
import {RouterService} from "../../../services/router.service";
import {AuthService} from "../../../services/auth.service";
import {SnackService} from "../../../services/snack.service";
import {IProblemDetails} from "../../../models/IProblemDetails";

@Component({
  selector: 'team-view',
  templateUrl: './team.view.component.html',
  styleUrls: ['./team.view.component.scss']
})
export class TeamViewComponent implements AfterViewInit {

  @Input() teamId?:number;

  public team: Team;
  public userId: number;

  isLoading: boolean = true;

  constructor(
    private activateRoute: ActivatedRoute,
    public router: RouterService,
    private teamsService: TeamService,
    private authService: AuthService,
    private snackService: SnackService) {

    this.userId = authService.getUserId() ?? 0;

    if (this.teamId == null)
      this.teamId = activateRoute.snapshot.params['teamId'];
  }

  ngAfterViewInit(): void {
    this.fetch();
  }

  fetch(){
    this.isLoading = true;
    this.teamsService.getById(this.teamId!)
      .pipe(finalize(()=>this.isLoading = false))
      .subscribe({
        next: (r: Team) =>  {
          this.team = r;
        },
        error: () => {}
      });
  }

  get canJoinToTeam() {
    return this.team
      && this.team.type == TeamType.Public
      && !this.teamContainsMember(this.team, this.userId);
  }

  get canLeaveTeam(){
    return this.team && this.teamContainsMember(this.team, this.userId);
  }

  get canSendRequestJoinToTeam(){
    return this.team
      && this.team.type == TeamType.Private
      && !this.teamContainsMember(this.team, this.userId);
  }

  public joinToTeam(): void {
    this.teamsService.joinToTeam(this.teamId ?? 0)
      .subscribe({
        next:() => {
          this.router.Teams.MyTeam();
        },
        error: err => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        }
      });
  }

  public sendRequestJoinToTeam():void{

  }

  public leaveTeam(): void
  {
    this.teamsService.leaveTeam(this.teamId ?? 0)
      .subscribe({
        next: () => {
          this.router.Teams.MyTeam();
        },
        error: err => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        }
      });
  }

  private teamContainsMember(team: Team, memberId: number):boolean
  {
    return (team.owner && team.owner.id == memberId) || (team.members && team.members.length > 0 && team.members.filter(x=>x.id == memberId).length > 0);
  }
}
