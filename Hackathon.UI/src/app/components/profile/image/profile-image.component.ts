import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { SnackService } from '../../../services/snack.service';
import { Subject, switchMap, takeUntil } from 'rxjs';
import { ProfileUserStore } from '../../../shared/stores/profile-user.store';
import { IUser } from '../../../models/User/IUser';
import { CurrentUserStore } from '../../../shared/stores/current-user.store';
import { UsersClient } from 'src/app/clients/users.client';

@Component({
  selector: 'profile-image',
  templateUrl: './profile-image.component.html',
  styleUrls: ['./profile-image.component.scss'],
})
export class ProfileImageComponent implements OnInit {
  @ViewChild('selectedFile') selectedFile: HTMLInputElement;
  @Input() canUpload: boolean = false;
  @Input() needUpdateUser: boolean = false;
  @Input() user: IUser;
  @Input() userId: number;

  private destroy$ = new Subject();

  constructor(
    private profileUserStore: ProfileUserStore,
    private snackService: SnackService,
    private usersClient: UsersClient,
    private currentUserStore: CurrentUserStore,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  public onImgError(): void {
    this.user.image = null;
  }

  public selectFile(event: Event): void {
    const target: HTMLInputElement = event.target as HTMLInputElement;
    const files: FileList = target.files;

    this.usersClient
      .setImage(files)
      .pipe(
        switchMap((imageId: string) => this.profileUserStore.updateUserUrl(this.user, imageId)),
        takeUntil(this.destroy$),
      )
      .subscribe((res: IUser) => {
        this.currentUserStore.loadCurrentUser(true);
        this.user = res;
      });
  }

  private loadData(): void {
    if (this.userId) {
      this.profileUserStore
        .getUser(this.userId, this.needUpdateUser)
        .pipe(takeUntil(this.destroy$))
        .subscribe((user: IUser) => (this.user = user));
    } else if (this.user) {
      this.profileUserStore
        .updateUserUrl(this.user, this.user.profileImageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe((user: IUser) => (this.user = user));
    }
  }
}
