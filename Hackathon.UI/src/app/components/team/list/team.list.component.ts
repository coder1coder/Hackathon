import {Component, Injectable} from '@angular/core';
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {TeamModel} from "../../../models/Team/TeamModel";
import {TeamService} from "../../../services/team.service";
import {AuthService} from "../../../services/auth.service";
import {GetFilterModel} from 'src/app/models/GetFilterModel';
import {FormControl, FormGroup} from '@angular/forms';
import {TeamFilterModel} from 'src/app/models/Team/TeamFilterModel';
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'team-list',
  templateUrl: './team.list.component.html',
  styleUrls: ['./team.list.component.scss']
})

@Injectable()
export class TeamListComponent extends BaseTableListComponent<TeamModel> {

  userId!:number | undefined;

  form = new FormGroup({
    teamName: new FormControl(),
    owner: new FormControl(),
    QuantityUsersFrom: new FormControl(),
    QuantityUsersTo: new FormControl()
  })

  constructor(
    private teamService: TeamService,
    private authService: AuthService,
    private router: RouterService) {
    super(TeamListComponent.name);
    this.userId = authService.getUserId();
  }

  createNewItem(){
    this.router.Teams.New();
  }

  override fetch(){
    let teamFilterModel = new TeamFilterModel();
    teamFilterModel.name =  this.form.get('teamName')?.value;
    teamFilterModel.owner =  this.form.get('owner')?.value;
    teamFilterModel.hasOwner = true;
    teamFilterModel.QuantityUsersFrom =  this.form.get('QuantityUsersFrom')?.value;
    teamFilterModel.QuantityUsersTo =  this.form.get('QuantityUsersTo')?.value;

    let getFilterModel = new GetFilterModel<TeamFilterModel>();
    getFilterModel.Page = this.pageSettings.pageIndex;
    getFilterModel.PageSize = this.pageSettings.pageSize;
    getFilterModel.Filter = teamFilterModel;

    this.teamService.getByFilter(getFilterModel)
      .subscribe({
        next: (r: BaseCollectionModel<TeamModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  clearFilter() {
    this.form.reset('teamName');
    this.form.reset('owner');
    this.form.reset('QuantityFrom');
    this.form.reset('QuantityTo');
    this.fetch();
  }

  rowClick = (item: TeamModel) => this.router.Teams.View(item.id);
  getDisplayColumns = (): string[] => ['name', 'owner', 'users', 'actions'];
}
