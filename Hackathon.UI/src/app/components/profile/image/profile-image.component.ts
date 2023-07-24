import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {SnackService} from "../../../services/snack.service";
import {UserService} from "../../../services/user.service";
import {BehaviorSubject, filter, mergeMap, of, Subject, switchMap, takeUntil} from "rxjs";
import {SafeUrl} from "@angular/platform-browser";
import {IUser} from "../../../models/User/IUser";
import {AuthService} from "../../../services/auth.service";
import {FileStorageService} from "src/app/services/file-storage.service";
import {UploadFileErrorMessages} from "../../../common/error-messages/upload-file-error-messages";
import { IProblemDetails } from "src/app/models/IProblemDetails";
import { ErrorProcessor } from "src/app/services/errorProcessor";

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
    private fileStorageService: FileStorageService,
    private errorProcessor: ErrorProcessor
    ) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  public selectFile(event: Event): void {
    const target = event.target as HTMLInputElement;
    const files = target.files as FileList;

    this.userService.setImage(files)
      .pipe(
        takeUntil(this.destroy$),
        mergeMap((imageId: string) => this.fileStorageService.getById(imageId))
      )
      .subscribe({
        next: (res: SafeUrl) => this.image = res,
        error: err => {
          this.errorProcessor.Process(err);
        }}
      );
  }

  private loadData(): void {
    this._userId
      .pipe(
        takeUntil(this.destroy$),
        filter((v) => Boolean(v)),
        switchMap(userId => this.userService.getById(userId)),
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
