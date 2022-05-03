import {Component} from '@angular/core';
import {UserService} from "../../../services/user.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {GetListParameters} from "../../../models/GetListParameters";
import {AuthService} from "../../../services/auth.service";
import {UserFilter} from "../../../models/User/UserFilter";
import {IUser} from "../../../models/User/IUser";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})

export class UserListComponent extends BaseTableListComponent<IUser>{

  constructor(private usersService: UserService,
              private routerService: RouterService,
              private authService: AuthService) {
    super(UserListComponent.name);
  }

  getDisplayColumns(): string[] {
    return ['userName', 'email', 'fullName', 'actions'];
  }

  override fetch(){

    let filter = new UserFilter();

    this.authService.getCurrentUser()?.subscribe(x => {
      if (x?.id != null)
        filter.excludeIds = [x.id];

      let getFilterModel = new GetListParameters<UserFilter>();
      getFilterModel.Offset = this.pageSettings.pageIndex;
      getFilterModel.Limit = this.pageSettings.pageSize;
      getFilterModel.Filter = filter;

      this.usersService.getList(getFilterModel)
        .subscribe({
          next: (r: BaseCollection<IUser>) =>  {
            this.items = r.items;
            this.pageSettings.length = r.totalCount;
          },
          error: () => {}
        });

    });
  }

  rowClick(user: IUser){
    if (user.id)
      this.routerService.Users.View(user.id);
  }
}
