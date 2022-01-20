import {AfterViewInit, Component} from '@angular/core';
import {Router} from "@angular/router";
import {PageSettings, PageSettingsDefaults} from "../../../models/PageSettings";
import {UserService} from "../../../services/user.service";
import {UserModel} from "../../../models/User/UserModel";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {PageEvent} from "@angular/material/paginator";


@Component({
  selector: 'user-list',
  templateUrl: './user.list.component.html',
  styleUrls: ['./user.list.component.scss']
})
export class UserListComponent implements AfterViewInit {

  users: UserModel[] = [];
  displayedColumns: string[] = ['id', 'userName', 'email', 'fullName', 'actions'];
  pageSettings: PageEvent = new PageEvent();

  constructor(private usersService: UserService, private router: Router) {

    let pageSettingsJson = sessionStorage.getItem(`${UserListComponent.name}${PageEvent.name}`);


    if (pageSettingsJson != null)
      this.pageUserSettings = JSON.parse(pageSettingsJson)
    else
    {
      this.pageUserSettings.pageSize = PageSettingsDefaults.PageSize;
      this.pageUserSettings.pageIndex = PageSettingsDefaults.PageIndex;
    }
  }

  setPageSettings(pageEvent:PageEvent){
    this.pageSettings = pageEvent;
    sessionStorage.setItem(`${UserListComponent.name}${PageEvent.name}`, JSON.stringify(pageEvent));

    this.fetch();
  }

  ngAfterViewInit(): void {
    this.fetch();
  }

  fetch(){
    this.usersService.getAll(new PageSettings(this.pageSettings))
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
}
