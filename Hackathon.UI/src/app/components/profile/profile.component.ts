import { AfterViewInit, Component, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { UserModel } from "../../models/User/UserModel";
import { FormBuilder, FormGroup } from "@angular/forms";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import { UserService } from "src/app/services/user.service";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { map } from "rxjs";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements OnInit, AfterViewInit {
  public image: any;
  public form = new FormGroup({})

  private currentUser: UserModel = new UserModel();
  private UserRoleTranslator: UserRoleTranslator = UserRoleTranslator;

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private userService: UserService,
    private sanitazer: DomSanitizer
    ) {
  }

  ngOnInit(): void {
    this.initForm();
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
}
