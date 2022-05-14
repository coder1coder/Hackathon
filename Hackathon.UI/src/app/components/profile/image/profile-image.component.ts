import { AfterViewInit, Attribute, Component, ViewChild } from "@angular/core";
import { SnackService } from "../../../services/snack.service";
import { UserService } from "../../../services/user.service";
import { mergeMap } from "rxjs";
import { DomSanitizer , SafeUrl } from "@angular/platform-browser";
import { map } from "rxjs/operators";
import { IUser } from "../../../models/User/IUser";
import { AuthService } from "../../../services/auth.service";
import { FileStorageService } from "src/app/services/file-storage.service";

@Component({
  selector: 'profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls:['./profile-image.component.scss']
})

export class ProfileImageComponent implements AfterViewInit {
  public image: any;
  public userNameSymbols: string = '';

  private maxFileSize: number = 2048;
  private sizeInMB: number = 1024;

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;
  constructor(
    private snackService: SnackService,
    private userService: UserService,
    private sanitizer: DomSanitizer,
    private authService: AuthService,
    private fileStorageService: FileStorageService,
    @Attribute('canUpload') public canUpload: boolean
    ) {
  }

  ngAfterViewInit(): void {
    this.authService.getCurrentUser()
      ?.subscribe((x: IUser) => {
        this.userNameSymbols = x.userName?.split(' ')
            .reduce((x,y) => x.concat(y))
            .substring(0,2)
            .toUpperCase()
          ?? '';

        if (x?.profileImageId != undefined)
          this.loadImage(x.profileImageId);
      })
  }

  public selectFile(event: any): void {
    if ( !(event?.target?.files?.length > 0) )
      return;

    let file:File = event.target.files[0];

    if (!ProfileImageComponent.isFileImage(file)) {
      this.snackService.open('Файл не является картинкой');
      return;
    }

    if (file.size / this.sizeInMB > this.maxFileSize) {
      this.snackService.open('Максимальный объем файла 2МБ');
      return;
    }

    this.fileStorageService.setImage(file)
      .pipe(
        mergeMap((res: string) =>
          this.fileStorageService.getImage(res)
            .pipe(
              map((res: ArrayBuffer) => this.getSafeUrlFromByteArray(res))
            ))
      )
      .subscribe((res : SafeUrl) => this.image = res)
  }

  public loadImage(imageId : string): void {
    this.fileStorageService.getImage(imageId)
        .pipe(map((x: ArrayBuffer) => this.getSafeUrlFromByteArray(x)))
        .subscribe(x => this.image = x);
  }

  private static isFileImage(file: File): boolean {
    return file && file['type'].split('/')[0] === 'image';
  }

  private getSafeUrlFromByteArray(buffer: ArrayBuffer): SafeUrl {
    let TYPED_ARRAY = new Uint8Array(buffer);
    const STRING_CHAR = TYPED_ARRAY.reduce((data, byte)=> data + String.fromCharCode(byte), '');
    let base64String = btoa(STRING_CHAR);

    return this.sanitizer.bypassSecurityTrustUrl('data:image/jpg;base64,' + base64String);
  }
}
