import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {BehaviorSubject, filter, mergeMap, of, Subject, switchMap, takeUntil} from "rxjs";
import {SafeUrl} from "@angular/platform-browser";
import {FileStorageService} from "src/app/services/file-storage.service";
import { ErrorProcessor } from "src/app/services/errorProcessor";
import { UserService } from "src/app/services/user.service";
import {IUser} from "../../../../models/User/IUser";

@Component({
  selector: 'profile-image',
  template: `
    <div class="noSelect" *ngIf="!image" [innerText]="userNameSymbols"></div>
    <img *ngIf="image" [src]="image" alt="">

    <input type="file" #selectedFile (change)="selectFile($event)">
    <button (click)="selectedFile.click()" *ngIf="canUpload">
        <mat-icon>photo_camera</mat-icon>
    </button>
  `,
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
    private userService: UserService,
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
