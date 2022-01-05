import {AfterViewInit, Component} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {UserModel} from "../../models/User/UserModel";
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements AfterViewInit {

  currentUser:UserModel | undefined;

  form = new FormGroup({
    id: new FormControl(),
    fullName: new FormControl(),
    email: new FormControl(),
    userName: new FormControl(),
  })

  constructor(private authService: AuthService) {
  }

  ngAfterViewInit(): void {

    let request = this.authService.getCurrentUser();

    if (request != undefined)
    {
      request.subscribe({
        next: r=> {
          this.currentUser = r;

          this.form = new FormGroup({
            id: new FormControl(r.id),
            fullName: new FormControl(r.fullName),
            email: new FormControl(r.email),
            userName: new FormControl(r.userName),
          })

        }
      })
    }
  }

  submit(){

  }
}
