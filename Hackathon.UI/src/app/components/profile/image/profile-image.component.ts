import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {SnackService} from "../../../services/snack.service";
import {UserService} from "../../../services/user.service";
import {BehaviorSubject, mergeMap} from "rxjs";
import {SafeUrl} from "@angular/platform-browser";
import {IUser} from "../../../models/User/IUser";
import {AuthService} from "../../../services/auth.service";
import {FileStorageService} from "src/app/services/file-storage.service";
import {FileUtils} from "../../../common/FileUtils";

@Component({
  selector: 'profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls:['./profile-image.component.scss']
})

export class ProfileImageComponent implements OnInit {

  @Input('canUpload') canUpload: boolean = false;

  private _userId = new BehaviorSubject<number>(0);

  @Input()
  set userId(value) { this._userId.next(value); };
  get userId() { return this._userId.getValue(); }

  public image: any;
  public userNameSymbols: string = '';

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;
  constructor(
    private snackService: SnackService,
    private userService: UserService,
    private authService: AuthService,
    private fileStorageService: FileStorageService
    ) {
  }

  public selectFile(event: any): void {
    if ( !(event?.target?.files?.length > 0) )
      return;

    let file:File = event.target.files[0];

    if (!FileUtils.IsImage(file)) {
      this.snackService.open('Файл не является картинкой');
      return;
    }

    if (file.size / FileUtils.Divider > FileUtils.MaxFileSize) {
      this.snackService.open('Максимальный объем файла 2МБ');
      return;
    }

    this.userService.setImage(file)
      .pipe(
        mergeMap((res: string) =>
          this.fileStorageService.getById(res))
      )
      .subscribe((res : SafeUrl) => this.image = res)
  }

  public loadImage(imageId : string): void {
    this.fileStorageService.getById(imageId)
        .subscribe(x => this.image = x);
  }

  ngOnInit(): void {

    this._userId.subscribe(userId=>{

      this.userService.getById(userId)
        ?.subscribe((x: IUser) => {

          this.userNameSymbols = x.userName?.split(' ')
              .reduce((x,y) => x.concat(y))
              .substring(0,2)
              .toUpperCase()
            ?? '';

          if (x?.profileImageId != undefined)
            this.loadImage(x.profileImageId);
        })
    })

  }
}
