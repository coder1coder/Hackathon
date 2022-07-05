import { Component, AfterViewInit } from "@angular/core";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import { AuthService } from "../../../services/auth.service";
import { KeyValue } from "@angular/common";
import { TeamService } from "../../../services/team.service";
import { catchError, of, switchMap } from "rxjs";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit {

  UserRoleTranslator = UserRoleTranslator;

  public userProfileDetails: KeyValue<string,any>[] = [];

  constructor(
    private authService: AuthService,
    private teamService: TeamService
  ) {}

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()
    ?.pipe(
      switchMap((res)=> {
        this.userProfileDetails = [
          { key: 'Имя пользователя', value: res.userName!},
          { key: 'Полное имя', value: res.fullName!},
          { key: 'E-mail', value: res.email!},
          { key: 'Роль', value: res.role!.toString()},
        ]
        return this.teamService.getMyTeam()
      }),
      catchError(() => {
        return of(null)
      })
    ).subscribe((res) => {
        this.userProfileDetails.push({ key: 'Команда', value: res?.name ?? "Не состоит в команде" })
      }
    )
  }
}
