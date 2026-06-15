import { Component, Injectable, inject } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { BaseTableListComponent } from '../../../common/base-components/base-table-list.component';
import { Team, TeamType } from '../../../models/Team/Team';
import { AuthService } from '../../../services/auth.service';
import { GetListParameters } from 'src/app/models/GetListParameters';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TeamFilter } from 'src/app/models/Team/TeamFilter';
import { RouterService } from '../../../services/router.service';
import { takeUntil } from 'rxjs';
import { TeamsClient } from 'src/app/clients/teams.client';
import { HttpErrorResponse } from '@angular/common/http';
import { DefaultLayoutComponent } from '../../layouts/default/default.layout.component';

import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatFormField, MatLabel, MatInput } from '@angular/material/input';
import { MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow } from '@angular/material/table';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { MatPaginator } from '@angular/material/paginator';

@Component({
    selector: 'team-list',
    templateUrl: './team.list.component.html',
    styleUrls: ['./team.list.component.scss'],
    imports: [DefaultLayoutComponent, MatButton, MatIcon, FormsModule, ReactiveFormsModule, MatFormField, MatLabel, MatInput, MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatIconButton, MatMenuTrigger, MatMenu, MatMenuItem, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow, MatPaginator]
})
@Injectable()
export class TeamListComponent extends BaseTableListComponent<Team> {
  private teamsClient = inject(TeamsClient);
  private authService = inject(AuthService);
  private router = inject(RouterService);
  private fb = inject(FormBuilder);

  public userId: number | null = this.authService.getUserId();
  public form = this.fb.group({
    teamName: [''],
    owner: [''],
    QuantityUsersFrom: [0],
    QuantityUsersTo: [0],
  });

  public canCreateNewTeam: boolean | undefined;

  constructor() {
    super(TeamListComponent.name);
  }

  public createNewItem = (): Promise<boolean> => this.router.Teams.New();
  public getDisplayColumns = (): string[] => ['name', 'owner', 'users', 'type', 'actions'];

  protected override onInit(): void {
    this.teamsClient
      .getMyTeam()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        error: (err: HttpErrorResponse) => {
          if (err.status === 404) {
            this.canCreateNewTeam = true;
          }
        },
      });
  }

  public override fetch(): void {
    //TODO: remove type assertion
    const teamFilterModel: TeamFilter = new TeamFilter();
    teamFilterModel.name = this.form.get('teamName')?.value as string;
    teamFilterModel.owner = this.form.get('owner')?.value as string;
    teamFilterModel.hasOwner = true;
    teamFilterModel.quantityMembersFrom = this.form.get('QuantityUsersFrom')?.value as number;
    teamFilterModel.quantityMembersTo = this.form.get('QuantityUsersTo')?.value as number;

    const getFilterModel: GetListParameters<TeamFilter> = new GetListParameters<TeamFilter>();
    getFilterModel.Offset = this.pageSettings.pageIndex;
    getFilterModel.Limit = this.pageSettings.pageSize;
    getFilterModel.Filter = teamFilterModel;

    this.teamsClient
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
