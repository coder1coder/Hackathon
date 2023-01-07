import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {SnackService} from "../../../services/snack.service";
import {UserService} from "../../../services/user.service";
import {BehaviorSubject, filter, mergeMap, of, Subject, switchMap, takeUntil} from "rxjs";
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
  @Input()
  set userId(value) { this._userId.next(value); };
  get userId() { return this._userId.getValue(); }

  public image: SafeUrl;
  public userNameSymbols: string = '';

  private _userId = new BehaviorSubject<number>(0);
  private destroy$: Subject<boolean> = new Subject<boolean>();

  @ViewChild('selectedFile') selectedFile: HTMLInputElement | undefined;
  constructor(
    private snackService: SnackService,
    private userService: UserService,
    private authService: AuthService,
    private fileStorageService: FileStorageService
    ) {
  }

  ngOnInit(): void {
    this.loadData();
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
        takeUntil(this.destroy$),
        mergeMap((res: string) =>
          this.fileStorageService.getById(res))
      )
      .subscribe((res : SafeUrl) => this.image = res)
  }

  private loadData(): void {
    this._userId
      .pipe(
        takeUntil(this.destroy$),
        filter((v) => Boolean(v)),
        switchMap(userId => {
          return this.userService.getById(userId);
        }),
        switchMap((user: IUser) => {
          this.userNameSymbols = user.userName?.split(' ')
              .reduce((x,y) => x.concat(y))
              .substring(0,2)
              .toUpperCase()
            ?? '';

          return user.profileImageId != undefined ? this.fileStorageService.getById(user.profileImageId) : of(null);
        })
      )
      .subscribe((url: SafeUrl | null) => {
        if (url) {
          this.image = url;
        }
      })
  }

}
