import {Component} from '@angular/core';
import {PageSettings} from "../../../models/PageSettings";
import {UserService} from "../../../services/user.service";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {IUserModel} from "../../../models/User/IUserModel";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})

export class UserListComponent extends BaseTableListComponent<IUserModel>{

  constructor(private usersService: UserService,
              private routerService: RouterService) {
    super(UserListComponent.name);
  }

  getDisplayColumns(): string[] {
    return ['userName', 'email', 'fullName', 'actions'];
  }

  fetch(){
    this.usersService.getAll(new PageSettings(this.pageSettings))
      .subscribe({
        next: (r: BaseCollectionModel<IUserModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  rowClick(user: IUserModel){
    if (user.id)
      this.routerService.Users.View(user.id);
  }
}
