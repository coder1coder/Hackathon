import {AfterViewInit, Component, Input} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {Team} from "../../../models/Team/Team";
import {TeamService} from "../../../services/team.service";
import {finalize} from "rxjs/operators";
import {MatTableDataSource} from "@angular/material/table";
import {RouterService} from "../../../services/router.service";
import {KeyValue} from "@angular/common";

@Component({
  selector: 'team-view',
  templateUrl: './team.view.component.html',
  styleUrls: ['./team.view.component.scss']
})
export class TeamViewComponent implements AfterViewInit {

  @Input() teamId?:number;

  public team: Team;
  teamDataSource = new MatTableDataSource<KeyValue<string, string>>([]);

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
        next: (r: Team) =>  {
          this.team = r;
          this.teamDataSource.data = [
            { key: 'Наименование',  value: this.team.name ?? '' }
          ];
        },
        error: () => {}
      });
  }
}
