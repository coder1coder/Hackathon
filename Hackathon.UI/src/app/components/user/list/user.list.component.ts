import {Component} from '@angular/core';
import {UserService} from "../../../services/user.service";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {GetFilterModel} from "../../../models/GetFilterModel";
import {AuthService} from "../../../services/auth.service";
import {UserFilterModel} from "../../../models/User/UserFilterModel";
import {IUserModel} from "../../../models/User/IUserModel";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})

export class UserListComponent extends BaseTableListComponent<IUserModel>{

  constructor(private usersService: UserService,
              private routerService: RouterService,
              private authService: AuthService) {
    super(UserListComponent.name);
  }

  getDisplayColumns(): string[] {
    return ['userName', 'email', 'fullName', 'actions'];
  }

  override fetch(){

    let filter = new UserFilterModel();

    this.authService.getCurrentUser()?.subscribe(x => {
      if (x?.id != null)
        filter.excludeIds = [x.id];

      let getFilterModel = new GetFilterModel<UserFilterModel>();
      getFilterModel.Page = this.pageSettings.pageIndex;
      getFilterModel.PageSize = this.pageSettings.pageSize;
      getFilterModel.Filter = filter;

      this.usersService.getList(getFilterModel)
        .subscribe({
          next: (r: BaseCollectionModel<IUserModel>) =>  {
            this.items = r.items;
            this.pageSettings.length = r.totalCount;
          },
          error: () => {}
        });

    });
  }

  rowClick(user: IUserModel){
    if (user.id)
      this.routerService.Users.View(user.id);
  }
}
