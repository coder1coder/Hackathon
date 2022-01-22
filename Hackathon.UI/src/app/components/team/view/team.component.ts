import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute} from "@angular/router";
import {TeamModel} from "../../../models/Team/TeamModel";
import {TeamService} from "../../../services/team.service";

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamViewComponent implements AfterViewInit {

  teamId: number;
  team: TeamModel | undefined;

  constructor(
    private activateRoute: ActivatedRoute,
    private teamsService: TeamService,
    private snackBar: MatSnackBar) {

    this.teamId = activateRoute.snapshot.params['eventId'];

    this.teamsService.getById(this.teamId)
      .subscribe({
        next: (r: TeamModel) =>  {
          this.team = r;
        },
        error: () => {}
      });

    this.teamsService = teamsService;
    this.snackBar = snackBar;
  }

  ngAfterViewInit(): void {
  }
}
