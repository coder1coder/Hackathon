import { Component, OnInit } from "@angular/core";
import { RouterService } from "../../../services/router.service";
import { TeamService } from "../../../services/team.service";
import { Team } from "../../../models/Team/Team";
import { AuthService } from "../../../services/auth.service";
import { Router } from "@angular/router";
import {finalize} from "rxjs/operators";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss']
})

export class UserTeamComponent implements OnInit
{
  public team?:Team;
  public isLoading = true;

  constructor(
    public routerService: RouterService,
    private teamService: TeamService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
      if (!this.authService.isLoggedIn())
      return;

    this.teamService.getMyTeam()
      .pipe(
        finalize(() => this.isLoading = false),
      )
      .subscribe({
        next: (res) =>
        {
          this.team = res;
        },
        error: (_) => {
          this.isLoading = false;
        }
      });
  }

  public leaveTeam(): void  {
    if (this.team !== undefined) {
      this.teamService.leaveTeam(this.team?.id)
        .subscribe(() => {
          this.router.routeReuseStrategy.shouldReuseRoute = () => false;
          this.router.navigate([this.router.url]);
        });
    }
  }
}
