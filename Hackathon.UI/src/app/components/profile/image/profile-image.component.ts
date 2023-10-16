import { Component, Input, OnInit, ViewChild } from "@angular/core";
import { SnackService } from "../../../services/snack.service";
import { UserService } from "../../../services/user.service";
import { Subject, takeUntil, tap } from "rxjs";
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
  @Input() userId: number;

  public user: IUser;
  private destroy$ = new Subject();

  constructor(
    private profileUserStore: ProfileUserStore,
    private snackService: SnackService,
    private userService: UserService,
  ) {}

  ngOnInit(): void {
    if (this.userId) this.loadData();
  }

  public onImgError(): void {
    this.user.image = null;
  }

  public selectFile(event: Event): void {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    this.userService.setImage(files)
      .pipe(
        tap((imageId: string) => this.profileUserStore.updateUserUrl(this.userId, imageId)),
        takeUntil(this.destroy$),
      ).subscribe(() => this.loadData(true))
  }

  private loadData(needUpdate = false): void {
    this.profileUserStore.getUser(this.userId, needUpdate)
      .pipe(takeUntil(this.destroy$))
      .subscribe((user: IUser) => this.user = user);
  }
}
