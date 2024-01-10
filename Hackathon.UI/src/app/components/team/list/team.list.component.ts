import { Component, Injectable } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { BaseTableListComponent } from '../../../common/base-components/base-table-list.component';
import { Team, TeamType } from '../../../models/Team/Team';
import { TeamClient } from '../../../services/team-client.service';
import { AuthService } from '../../../services/auth.service';
import { GetListParameters } from 'src/app/models/GetListParameters';
import { FormBuilder } from '@angular/forms';
import { TeamFilter } from 'src/app/models/Team/TeamFilter';
import { RouterService } from '../../../services/router.service';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'team-list',
  templateUrl: './team.list.component.html',
  styleUrls: ['./team.list.component.scss'],
})
@Injectable()
export class TeamListComponent extends BaseTableListComponent<Team> {
  public userId: number | undefined = this.authService.getUserId();
  public form = this.fb.group({
    teamName: [null],
    owner: [null],
    QuantityUsersFrom: [null],
    QuantityUsersTo: [null],
  });

  constructor(
    private teamService: TeamClient,
    private authService: AuthService,
    private router: RouterService,
    private fb: FormBuilder,
  ) {
    super(TeamListComponent.name);
  }

  public createNewItem = (): Promise<boolean> => this.router.Teams.New();
  public getDisplayColumns = (): string[] => ['name', 'owner', 'users', 'type', 'actions'];

  public override fetch(): void {
    const teamFilterModel: TeamFilter = new TeamFilter();
    teamFilterModel.name = this.form.get('teamName')?.value;
    teamFilterModel.owner = this.form.get('owner')?.value;
    teamFilterModel.hasOwner = true;
    teamFilterModel.quantityMembersFrom = this.form.get('QuantityUsersFrom')?.value;
    teamFilterModel.quantityMembersTo = this.form.get('QuantityUsersTo')?.value;

    const getFilterModel: GetListParameters<TeamFilter> = new GetListParameters<TeamFilter>();
    getFilterModel.Offset = this.pageSettings.pageIndex;
    getFilterModel.Limit = this.pageSettings.pageSize;
    getFilterModel.Filter = teamFilterModel;

    this.teamService
      .getByFilter(getFilterModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (r: BaseCollection<Team>) => {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {},
      });
  }

  public clearFilter(): void {
    this.form.reset();
    this.fetch();
  }

  public rowClick = (item: Team): void => {
    if (item.owner?.id === this.userId) {
      this.router.Teams.MyTeam();
    } else {
      this.router.Teams.View(item.id);
    }
  };

  public getTeamTypeName(type: TeamType): string {
    switch (type) {
      case TeamType.Private:
        return 'Закрытый';
      case TeamType.Public:
        return 'Открытый';
      default:
        return '';
    }
  }
}
