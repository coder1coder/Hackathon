import {Component, Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {PageSettings} from "../../../models/PageSettings";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {TeamModel} from "../../../models/Team/TeamModel";
import {TeamService} from "../../../services/team.service";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'team-list',
  templateUrl: './team.list.component.html',
  styleUrls: ['./team.list.component.scss']
})

@Injectable()
export class TeamListComponent extends BaseTableListComponent<TeamModel> {

  public userId:number | undefined;

  constructor(
    private teamService: TeamService,
    private authService: AuthService,
    private router: Router) {
    super(TeamListComponent.name);
    this.userId = authService.getUserId();
  }

  createNewItem(){
    this.router.navigate(['/teams/new']);
  }

  override fetch(){
    this.teamService.getAll(new PageSettings(this.pageSettings))
      .subscribe({
        next: (r: BaseCollectionModel<TeamModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  rowClick(item: TeamModel){

  }

  getDisplayColumns(): string[] {
    return ['id', 'name', 'owner', 'actions'];
  }
}
