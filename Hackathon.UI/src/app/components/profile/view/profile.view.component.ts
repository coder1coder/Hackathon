import {Component, ViewChild, AfterViewInit} from "@angular/core";
import { FormBuilder } from "@angular/forms";
import { UserRoleTranslator } from "src/app/models/User/UserRole";
import { UserService } from "src/app/services/user.service";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";
import { map, mergeMap } from "rxjs";
import { SnackService } from "src/app/services/snack.service";
import {IUser} from "../../../models/User/IUser";
import {environment} from "../../../../environments/environment";
import {AuthService} from "../../../services/auth.service";
import {KeyValue} from "@angular/common";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit {

  api = environment.api;

  UserRoleTranslator = UserRoleTranslator;

  public image: any;
  public userNameSymbols:string = '';
  public userProfileDetails: KeyValue<string,any>[] = [];

  constructor(
    private authService: AuthService,
    private fb: FormBuilder,
    private userService: UserService,
    private sanitizer: DomSanitizer,
    private snackBar:SnackService
    ) {
  }

  readonly maxFileSize : number = 2048;

  ngAfterViewInit(): void {

    this.authService.getCurrentUser()?.subscribe((x: IUser) => {

      this.userNameSymbols = x.userName?.split(' ')
        .reduce((x,y) => x.concat(y))
        .substring(0,2)
        .toUpperCase()
        ?? '';

      this.userProfileDetails = [
        { key: 'Имя пользователя', value: x.userName!},
        { key: 'Полное имя', value: x.fullName!},
        { key: 'E-mail', value: x.email!},
        { key: 'Роль', value: x.role!.toString()}
      ]

      if (x?.profileImageId != undefined)
          this.loadImage(x.profileImageId);
      })
  }

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;

  public selectFile(event:any) {
    if ( !(event?.target?.files?.length > 0) )
      return;

    let file:File = event.target.files[0];

    if (!ProfileViewComponent.isFileImage(file))
    {
      this.snackBar.open('Файл не является картинкой');
      return;
    }

    if (file.size/1024 > this.maxFileSize)
    {
      this.snackBar.open('Максимальный объем файла 2МБ');
      return;
    }

    this.userService.setProfileImage(file)
    .pipe(
      mergeMap( (res : string) =>
        this.userService
          .getProfileImage(res)
          .pipe(
            map((res: ArrayBuffer) => this.getSafeUrlFromByteArray(res))
          ))
    )
    .subscribe( (res : SafeUrl) => this.image = res)
  }

  public loadImage(imageId : string): void {
    this.userService.getProfileImage(imageId)
        .pipe(map((x: ArrayBuffer) => this.getSafeUrlFromByteArray(x)))
        .subscribe(x => {

          this.image = x
          console.log(this.image)
        });
  }

  private static isFileImage(file: File) : boolean {
    return file && file['type'].split('/')[0] === 'image';
  }

  private getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    let TYPED_ARRAY = new Uint8Array(buffer);
    const STRING_CHAR = TYPED_ARRAY.reduce((data, byte)=> data + String.fromCharCode(byte), '');
    let base64String = btoa(STRING_CHAR);

    return this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
  }
}
