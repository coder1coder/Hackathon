import {AfterViewInit, Component} from '@angular/core';
import {Router} from "@angular/router";
import {PageSettings, PageSettingsDefaults} from "../../../models/PageSettings";
import {UserService} from "../../../services/user.service";
import {UserModel} from "../../../models/User/UserModel";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {PageUserSettings, PageUserSettingsDefaults} from "../../../models/PageUserSettings";
import {PageUser} from "@angular/material/paginator";
//import {PageUser} from "@angular/core";
import {PageEvent} from "@angular/material/paginator";

@Component({
  selector: 'user-list',
  //template: `<h2>user list</h2>`,
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})
export class UserListComponent implements AfterViewInit {

  users: UserModel[] = [];
  displayedColumns: string[] = ['id', 'userName', 'email', 'fullName', 'actions'];
  //pageSettings: PageUser = new PageUser();
  pageUserSettings: PageUser = new PageUser();

  constructor(private usersService: UserService, private router: Router) {

    let pageSettingsJson = sessionStorage.getItem(`${UserListComponent.name}${PageUser.name}`);


    if (pageSettingsJson != null)
      this.pageUserSettings = JSON.parse(pageSettingsJson)
    else
    {
      this.pageUserSettings.pageSize = PageSettingsDefaults.PageSize;
      this.pageUserSettings.pageIndex = PageSettingsDefaults.PageIndex;
    }
  }

  setPageSettings(user:PageUser){
    this.pageUserSettings = user;
    sessionStorage.setItem(`${UserListComponent.name}${PageUser.name}`, JSON.stringify(user));

    this.fetch();
  }

  ngAfterViewInit(): void {
    this.fetch();
  }

  fetch(){
    this.usersService.getAll(new PageUserSettings(this.pageUserSettings))
      .subscribe({
        next: (r: BaseCollectionModel<UserModel>) =>  {
          this.users = r.items;
          this.pageUserSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  handleRowClick(user: UserModel){
    this.router.navigate(['/users/'+user.id]);
  }

  // showUserPage(user: UserModel){
  //   this.router.navigate(['/user/'+user.id]);
  // }
}
