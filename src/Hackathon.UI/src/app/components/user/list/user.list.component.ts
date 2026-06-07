import { Component } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { BaseTableListComponent } from '../../../common/base-components/base-table-list.component';
import { GetListParameters } from '../../../models/GetListParameters';
import { UserFilter } from '../../../models/User/UserFilter';
import { IUser } from '../../../models/User/IUser';
import { RouterService } from '../../../services/router.service';
import { CurrentUserStore } from '../../../shared/stores/current-user.store';
import { fromMobx } from '../../../common/functions/from-mobx.function';
import { mergeMap, takeUntil } from 'rxjs';
import { UsersClient } from 'src/app/clients/users.client';
import { DefaultLayoutComponent } from '../../layouts/default/default.layout.component';
import { NgIf } from '@angular/common';
import { MatTable, MatColumnDef, MatHeaderCellDef, MatHeaderCell, MatCellDef, MatCell, MatHeaderRowDef, MatHeaderRow, MatRowDef, MatRow } from '@angular/material/table';
import { MatIconButton } from '@angular/material/button';
import { MatMenuTrigger, MatMenu, MatMenuItem } from '@angular/material/menu';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator } from '@angular/material/paginator';
import { ProfileImageComponent } from '../../profile/image/profile-image.component';

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss'],
  imports: [
    DefaultLayoutComponent,
    NgIf,
    MatTable,
    MatColumnDef,
    MatHeaderCellDef,
    MatHeaderCell,
    MatCellDef,
    MatCell,
    MatIconButton,
    MatMenuTrigger,
    MatIcon,
    MatMenu,
    MatMenuItem,
    MatHeaderRowDef,
    MatHeaderRow,
    MatRowDef,
    MatRow,
    MatPaginator,
    ProfileImageComponent,
  ],
})
export class UserListComponent extends BaseTableListComponent<IUser> {
  constructor(
    private usersClient: UsersClient,
    private routerService: RouterService,
    private currentUserStore: CurrentUserStore,
  ) {
    super(UserListComponent.name);
    this.currentUserStore.loadCurrentUser();
  }

  public getDisplayColumns(): string[] {
    return ['userName', 'email', 'fullName', 'actions'];
  }

  override fetch(): void {
    const userFilter: UserFilter = new UserFilter();
    //TODO: Remove type assertion
    fromMobx(() => this.currentUserStore.currentUser as IUser)
      .pipe(
        mergeMap((user: IUser) => {
          if (user?.id != null) {
            userFilter.excludeIds = [user.id];
          }

          const getFilterModel: GetListParameters<UserFilter> = new GetListParameters<UserFilter>();
          getFilterModel.Offset = this.pageSettings.pageIndex;
          getFilterModel.Limit = this.pageSettings.pageSize;
          getFilterModel.Filter = userFilter;

          return this.usersClient.getList(getFilterModel);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res: BaseCollection<IUser>) => {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
        error: () => {},
      });
  }

  public rowClick(user: IUser): void {
    if (user.id) {
      this.routerService.Users.View(user.id);
    }
  }
}
