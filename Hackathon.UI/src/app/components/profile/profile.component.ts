import { AfterViewInit, Component, ViewChild, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { UserModel } from "../../models/User/UserModel";
import { FormControl, FormGroup } from "@angular/forms";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { Bucket } from "../../common/Bucket";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements OnInit, AfterViewInit {
  public image: any;
  public form = new FormGroup({})

  api = environment.api;

  currentUser:UserModel = new UserModel();
  UserRoleTranslator = UserRoleTranslator;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private userService: UserService,
    private sanitazer: DomSanitizer
    ) {
  }

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;

  constructor(private authService: AuthService, private http:HttpClient) {
  }

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()
      ?.subscribe((user: UserModel) => {
        this.currentUser = user;
        this.loadForm();
      })

    // TODO Реализовать кнопку загрузки аватара (файла),
    // TODO хранить id файла в userModel,
    // TODO при инициализации компонента передавать fileId user в getProfileImage();
    const fileId = 'f2a4d265-e8d5-4970-8b68-1f73fa8276ec';

    this.userService.getProfileImage(fileId)
      .pipe(
        map((res: ArrayBuffer) => {
          return this.getSafeUrlFromByteArray(res);
        })
      )
      .subscribe(response => {
        this.image = response;
      });
  }

  public submit(): void {

  }

  private initForm(): void {
    this.form = this.fb.group({
      id: [null],
      fullName: [null],
      email: [null],
      userName: [null],
      role: [null],
    });
  }

  private loadForm(): void {
    if(this.currentUser) {
      this.form.patchValue({
        id: this.currentUser?.id,
        fullName: this.currentUser?.fullName,
        email: this.currentUser?.email,
        userName: this.currentUser?.userName,
        role: this.currentUser?.role,
       });
    }
  }

  private getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    let TYPED_ARRAY = new Uint8Array(buffer);
    const STRING_CHAR = TYPED_ARRAY.reduce((data, byte)=> {
      return data + String.fromCharCode(byte);
    }, '');
    let base64String = btoa(STRING_CHAR);

    return this.sanitazer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
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
