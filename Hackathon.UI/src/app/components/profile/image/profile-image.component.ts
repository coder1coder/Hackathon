import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { SnackService } from "../../../services/snack.service";
import { UserService } from "../../../services/user.service";
import { mergeMap, Subject, takeUntil } from "rxjs";
import { ProfileUserStore } from "../../../shared/stores/profile-user.store";
import { IUser } from "../../../models/User/IUser";

@Component({
  selector: 'profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls:['./profile-image.component.scss'],
})

export class ProfileImageComponent implements OnInit {

  @ViewChild('selectedFile') selectedFile: HTMLInputElement;
  @Input('canUpload') canUpload: boolean = false;
  @Input() user: IUser;
  @Input() userId: number;

  private destroy$ = new Subject();

  constructor(
    private profileUserStore: ProfileUserStore,
    private snackService: SnackService,
    private userService: UserService,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  public onImgError(): void {
    this.user.image = null;
  }

  public selectFile(event: Event): void {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    this.userService.setImage(files)
      .pipe(
        mergeMap((imageId: string) => this.profileUserStore.updateUserUrl(this.user, imageId)),
        takeUntil(this.destroy$),
      ).subscribe((res: IUser) => this.user = res);
  }

  private loadData(needUpdate = false): void {
    if (this.userId) {
      this.profileUserStore.getUser(this.userId, needUpdate)
        .pipe(takeUntil(this.destroy$))
        .subscribe((user: IUser) => this.user = user);
    } else if (this.user) {
      this.profileUserStore.updateUserUrl(this.user, this.user.profileImageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((res: IUser) => this.user = res);
    }
  }
}
