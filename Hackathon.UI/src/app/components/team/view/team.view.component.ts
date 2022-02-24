import {AfterViewInit, Component, Input} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {TeamModel} from "../../../models/Team/TeamModel";
import {TeamService} from "../../../services/team.service";
import {SnackService} from "../../../services/snack.service";
import {finalize} from "rxjs/operators";
import {MatTableDataSource} from "@angular/material/table";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'team-view',
  templateUrl: './team.view.component.html',
  styleUrls: ['./team.view.component.scss']
})
export class TeamViewComponent implements AfterViewInit {

  @Input() teamId?:number;

  team?: TeamModel;
  teamDataSource = new MatTableDataSource<{ key:string, value?:string }>([]);

  isLoading: boolean = true;

  constructor(
    private activateRoute: ActivatedRoute,
    public router: RouterService,
    private teamsService: TeamService) {

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
        next: (r: TeamModel) =>  {
          this.team = r;
          this.teamDataSource.data = [
            { key: 'Наименование', value: this.team.name }
          ];
        },
        error: () => {}
      });
  }
}
