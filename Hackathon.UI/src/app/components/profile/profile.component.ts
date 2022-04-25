import {AfterViewInit, Component, ViewChild} from "@angular/core";
import {AuthService} from "../../services/auth.service";
import {UserModel} from "../../models/User/UserModel";
import {FormControl, FormGroup} from "@angular/forms";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {Bucket} from "../../common/Bucket";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements AfterViewInit {

  api = environment.api;

  currentUser:UserModel = new UserModel();
  UserRoleTranslator = UserRoleTranslator;

  form = new FormGroup({
    id: new FormControl(),
    fullName: new FormControl(),
    email: new FormControl(),
    userName: new FormControl(),
    role: new FormControl(),
  })

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;

  constructor(private authService: AuthService, private http:HttpClient) {
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
            role: new FormControl(UserRoleTranslator.Translate(r.role)),
          })

        }
      })
    }
  }

  submit(){

  }

  selectFile(event:any) {

    if (event?.target?.files?.length > 0)
    {
      let file = event.target.files[0];

      const formData = new FormData();
      formData.append("file", file);

      this.http.post(this.api+'/FileStorage/Upload/'+Bucket.Avatars, formData, {
        headers: new HttpHeaders().set('Content-Disposition', 'multipart/form-data')
      }).subscribe(x=>{
        console.log(x)
      })
    }
  }
}
