import {Component, AfterViewInit} from "@angular/core";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import {IUser} from "../../../models/User/IUser";
import {AuthService} from "../../../services/auth.service";
import {KeyValue} from "@angular/common";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit {

  UserRoleTranslator = UserRoleTranslator;

  public userProfileDetails: KeyValue<string,any>[] = [];

  constructor(
    private authService: AuthService) {
  }

  ngAfterViewInit(): void {

    this.authService.getCurrentUser()?.subscribe((x: IUser) => {

      this.userProfileDetails = [
        { key: 'Имя пользователя', value: x.userName!},
        { key: 'Полное имя', value: x.fullName!},
        { key: 'E-mail', value: x.email!},
        { key: 'Роль', value: x.role!.toString()}
      ]
      })
  }

}
