import {Component, Injectable} from '@angular/core';
import {BaseCollection} from "../../../models/BaseCollection";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {Team} from "../../../models/Team/Team";
import {TeamService} from "../../../services/team.service";
import {AuthService} from "../../../services/auth.service";
import {GetListParameters} from 'src/app/models/GetListParameters';
import {FormControl, FormGroup} from '@angular/forms';
import {TeamFilter} from 'src/app/models/Team/TeamFilter';
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'team-list',
  templateUrl: './team.list.component.html',
  styleUrls: ['./team.list.component.scss']
})

@Injectable()
export class TeamListComponent extends BaseTableListComponent<Team> {

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
    let teamFilterModel = new TeamFilter();
    teamFilterModel.name =  this.form.get('teamName')?.value;
    teamFilterModel.owner =  this.form.get('owner')?.value;
    teamFilterModel.hasOwner = true;
    teamFilterModel.quantityMembersFrom =  this.form.get('QuantityUsersFrom')?.value;
    teamFilterModel.quantityMembersTo =  this.form.get('QuantityUsersTo')?.value;

    let getFilterModel = new GetListParameters<TeamFilter>();
    getFilterModel.Offset = this.pageSettings.pageIndex;
    getFilterModel.Limit = this.pageSettings.pageSize;
    getFilterModel.Filter = teamFilterModel;

    this.teamService.getByFilter(getFilterModel)
      .subscribe({
        next: (r: BaseCollection<Team>) =>  {
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

  rowClick = (item: Team) => this.router.Teams.View(item.id);
  getDisplayColumns = (): string[] => ['name', 'owner', 'users', 'actions'];
}
