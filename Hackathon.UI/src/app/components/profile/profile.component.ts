import { Component, ViewChild, OnInit } from "@angular/core";
import { AuthService } from "../../services/auth.service";
import { UserModel } from "../../models/User/UserModel";
import { FormBuilder, FormControl, FormGroup } from "@angular/forms";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import { environment } from "../../../environments/environment";
import { UserService } from "src/app/services/user.service";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { map, mergeMap } from "rxjs";
import { UploadedFileStorage } from "src/app/models/FileStorage/UploadedFileStorage";

@Component({
  templateUrl: './profile.component.html',
  styleUrls:['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
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

  ngOnInit(): void {
    this.initForm();

    this.authService.getCurrentUser()
      ?.subscribe((user: UserModel) => {
        this.currentUser = user;
        this.loadForm();

        if (this.currentUser.profileImageId != undefined) {
          this.loadImage(this.currentUser.profileImageId);
        }
      })

    // TODO Реализовать кнопку загрузки аватара (файла), +/- Done
    // TODO хранить id файла в userModel, +/- Done
    // TODO при инициализации компонента передавать fileId user в getProfileImage();
  }

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;

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

  public selectFile(event:any) {
    if (event?.target?.files?.length > 0)
    {
      let file:File = event.target.files[0];
      //формат файла - картика
      if (this.isFileImage(file) == false)
      {
        //вывести что эта картинка не подоходит.
        return;
      }

      //ограничить размер в ~2МБ,
      if (file.size/1024 > 2000) //2000КБ
      {
        //вывести что объем файла больше 2000КБ
      } 
      
      //<!-- 200x200 размер картинки-->
      this.userService.setProfileImage(file, this.currentUser.id)
      .pipe(
        mergeMap( (res : UserModel) => {
          return this.userService.getProfileImage(res.profileImageId!)
          .pipe(
            map((res: ArrayBuffer) => {
              return this.getSafeUrlFromByteArray(res);
            })
          )
        })
      )
      .subscribe( (res : SafeUrl) => {
        this.image = res;
      })
    }
  }

  public loadImage(imageId : string): void {
    this.userService.getProfileImage(imageId)
        .pipe(
          map((res: ArrayBuffer) => {
            return this.getSafeUrlFromByteArray(res);
          })
        )
        .subscribe(response => {
          this.image = response;
        });
  }

  private isFileImage(file: File) : boolean {
        return file && file['type'].split('/')[0] === 'image';
  }
}
