import {AfterViewInit, Component} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {UserModel} from "../../models/User/UserModel";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements AfterViewInit {

  currentUser:UserModel | undefined;

  constructor(private authService: AuthService) {
  }

  ngAfterViewInit(): void {

    let request = this.authService.getCurrentUser();

    if (request != undefined)
    {
      request.subscribe({
        next: r=> {
          this.currentUser = r;
        }
      })
    }
  }
}
