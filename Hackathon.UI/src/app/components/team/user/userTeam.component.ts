import {AfterViewInit, Component} from "@angular/core";
import {RouterService} from "../../../services/router.service";
import {TeamService} from "../../../services/team.service";
import {Team} from "../../../models/Team/Team";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss']
})

export class UserTeamComponent implements AfterViewInit
{
  team?:Team

  constructor(
    public router:RouterService,
    private teamService: TeamService,
    private authService:AuthService

  ) {}

  ngAfterViewInit(): void {
    if (!this.authService.isLoggedIn())
      return;

    this.teamService.getMyTeam().subscribe(r => this.team = r);
  }

}
