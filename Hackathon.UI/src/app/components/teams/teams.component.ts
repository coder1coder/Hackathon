import {AfterViewInit, Component} from '@angular/core';
import {BaseCollectionModel} from "../../models/BaseCollectionModel";
import {TeamModel} from "../../models/Team/TeamModel";
import {TeamsService} from "../../services/teams.service";
import {Router} from "@angular/router";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'teams',
  templateUrl: './teams.component.html',
  styleUrls: ['./teams.component.scss']
})
export class TeamsComponent implements AfterViewInit {

  teams: TeamModel[] = [];
  displayedColumns: string[] = ['id', 'name'];

  constructor(private teamsService: TeamsService, private router: Router) {
  }

  ngAfterViewInit(): void {
    this.teamsService.getAll()
      .subscribe({
        next: (r: BaseCollectionModel<TeamModel>) =>  {
          this.teams = r.items;
        },
        error: (e:HttpErrorResponse) => {
          console.log(e.error.status);
        }
      });

  }

  handleRowClick(team: TeamModel){
    this.router.navigate(['/team/'+team.id]);
  }
}
