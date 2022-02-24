import {AfterViewInit, Component} from "@angular/core";
import {RouterService} from "../../../services/router.service";
import {TeamService} from "../../../services/team.service";
import {TeamModel} from "../../../models/Team/TeamModel";

@Component({
  selector: 'userTeam',
  templateUrl: 'userTeam.component.html',
  styleUrls: ['userTeam.component.scss']
})

export class UserTeamComponent implements AfterViewInit
{
  team?:TeamModel

  constructor(
    public router:RouterService,
    private teamService: TeamService

  ) {}

  ngAfterViewInit(): void {
    this.teamService.getMyTeam().subscribe(r => this.team = r);
  }

}
