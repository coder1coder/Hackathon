import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {PageSettings} from "../../../models/PageSettings";
import {UserService} from "../../../services/user.service";
import {UserModel} from "../../../models/User/UserModel";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {BaseTableListComponent} from "../../BaseTableListComponent";

@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})

export class UserListComponent extends BaseTableListComponent<UserModel>{

  constructor(private usersService: UserService, private router: Router) {
    super(UserListComponent.name);
  }

  getDisplayColumns(): string[] {
    return ['id', 'userName', 'email', 'fullName', 'actions'];
  }

  fetch(){
    this.usersService.getAll(new PageSettings(this.pageSettings))
      .subscribe({
        next: (r: BaseCollectionModel<UserModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  rowClick(user: UserModel){
    this.router.navigate(['/users/'+user.id]);
  }
}
