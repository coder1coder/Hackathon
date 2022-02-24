import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute} from "@angular/router";
import {UserModel} from "../../../models/User/UserModel";
import {UserService} from "../../../services/user.service";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {SnackService} from "../../../services/snack.service";

@Component({
  selector: 'user-view',
  templateUrl: './user.view.component.html',
  styleUrls: ['./user.view.component.scss']
})
export class UserViewComponent implements AfterViewInit {

  userId: number;
  user?: UserModel;
  isLoading: boolean = true;

  constructor(
    private activateRoute: ActivatedRoute,
    private usersService: UserService,
    private authService: AuthService,
    private snackBar: SnackService,
  ) {
    this.userId = activateRoute.snapshot.params['userId'];
    this.usersService = usersService;
    this.snackBar = snackBar;
  }

  ngAfterViewInit(): void {
    this.fetchEvent();
  }

  fetchEvent() {
    this.isLoading = true;
    this.usersService.getById(this.userId)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (r: UserModel) => {
          this.user = r;
        },
        error: () => {
        }
      });
  }
}
