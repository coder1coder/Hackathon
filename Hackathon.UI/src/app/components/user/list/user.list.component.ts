import { Component } from '@angular/core';
import { UserService } from "../../../services/user.service";
import { BaseCollection } from "../../../models/BaseCollection";
import { BaseTableListComponent } from "../../../common/base-components/base-table-list.component";
import { GetListParameters } from "../../../models/GetListParameters";
import { UserFilter } from "../../../models/User/UserFilter";
import { IUser } from "../../../models/User/IUser";
import { RouterService } from "../../../services/router.service";
import { CurrentUserStore } from "../../../shared/stores/current-user.store";
import { fromMobx } from "../../../common/functions/from-mobx.function";
import { mergeMap, takeUntil } from "rxjs";

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss'],
})

export class UserListComponent extends BaseTableListComponent<IUser> {

  constructor(
    private usersService: UserService,
    private routerService: RouterService,
    private currentUserStore: CurrentUserStore,
  ) {
    super(UserListComponent.name);
  }

  public getDisplayColumns(): string[] {
    return ['userName', 'email', 'fullName', 'actions'];
  }

  override fetch(): void {
    const userFilter = new UserFilter();
    fromMobx(() => this.currentUserStore.currentUser)
      .pipe(
        mergeMap((user: IUser) => {
          if (user?.id != null) {
            userFilter.excludeIds = [user.id];
          }

          let getFilterModel = new GetListParameters<UserFilter>();
          getFilterModel.Offset = this.pageSettings.pageIndex;
          getFilterModel.Limit = this.pageSettings.pageSize;
          getFilterModel.Filter = userFilter;

          return this.usersService.getList(getFilterModel);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe(({
        next: (res: BaseCollection<IUser>) =>  {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
        error: () => {}
      }))
  }

  public rowClick(user: IUser): void {
    if (user.id) {
      this.routerService.Users.View(user.id);
    }
  }
}
