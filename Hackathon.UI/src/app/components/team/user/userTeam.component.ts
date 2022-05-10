import { Component, OnInit } from "@angular/core";
import { RouterService } from "../../../services/router.service";
import { TeamService } from "../../../services/team.service";
import { Team } from "../../../models/Team/Team";
import { AuthService } from "../../../services/auth.service";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss']
})

export class UserTeamComponent implements OnInit
{
  public team?:Team;

  constructor(
    public routerService: RouterService,
    private teamService: TeamService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
      if (!this.authService.isLoggedIn())
      return;

    this.teamService.getMyTeam()
      .subscribe({
        next: (res) => this.team = res,
        error: (error) => {
          this.team = undefined;
        }
      });
  }

  public leaveTeam(): void  {
    if (this.team !== undefined) {
      this.teamService.leaveTeam(this.team?.id)
        .subscribe(() => {
          this.ngOnInit();
        });
    }
  }
}
