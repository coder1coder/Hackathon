import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { FriendshipService } from '../../../services/friendship/friendship.service';
import { FriendshipStatus } from '../../../models/Friendship/FriendshipStatus';
import { BaseCollection } from '../../../models/BaseCollection';
import { IUser } from '../../../models/User/IUser';
import { RouterService } from '../../../services/router.service';
import { AuthService } from '../../../services/auth.service';
import { SignalRService } from '../../../services/signalr.service';

@Component({
  selector: `friends-list`,
  styleUrls: ['./friends-list.component.scss'],
  templateUrl: `./friends-list.component.html`,
})
export class FriendsListComponent implements OnInit, OnDestroy {
  @Input()
  set userId(value) {
    this._userId.next(value);
  }
  get userId(): number {
    return this._userId.getValue();
  }

  @Input() status: FriendshipStatus;
  public FriendshipStatus = FriendshipStatus;
  public Number = Number;
  public users: IUser[] = [];

  public authUserId: number;

  private _userId = new BehaviorSubject<number>(0);
  private destroy$ = new Subject();

  constructor(
    private friendshipService: FriendshipService,
    private routerService: RouterService,
    private authService: AuthService,
    private signalRService: SignalRService,
  ) {}

  ngOnInit(): void {
    this._userId.pipe(takeUntil(this.destroy$)).subscribe((x) => {
      this.fetchUsersByStatus(x, this.status);
    });

    this.authUserId = this.authService.getUserId();

    this.signalRService.onFriendshipChangedIntegration = (x): void => {
      if (
        this.authUserId !== undefined &&
        (x.userIds.indexOf(this.authUserId) >= 0 || x.userIds.indexOf(this.userId) >= 0)
      )
        this.fetchUsersByStatus(this.userId, this.status);
    };
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private fetchUsersByStatus(userId: number, status?: FriendshipStatus): void {
    if (status != undefined)
      this.friendshipService
        .getUsersByFriendshipStatus(userId, status)
        .pipe(takeUntil(this.destroy$))
        .subscribe((x: BaseCollection<IUser>) => {
          this.users = x.items;
        });
  }

  getNoDataText(): string {
    switch (this.status) {
      case FriendshipStatus.Pending:
        return `Заявки в друзья не найдены`;
      case FriendshipStatus.Confirmed:
        return `У вас еще нет друзей`;
      default:
        return `Нет данных`;
    }
  }

  public openUserProfile(userId: number): void {
    this.routerService.Users.View(userId);
  }

  public rejectOffer(userId: number): void {
    this.friendshipService
      .rejectOffer(userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchUsersByStatus(this.userId, this.status);
      });
  }

  public acceptOffer(userId: number): void {
    this.friendshipService
      .createOrAcceptOffer(userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchUsersByStatus(this.userId, this.status);
      });
  }

  public removeFromFriends(userId: number): void {
    this.friendshipService
      .endFriendship(userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchUsersByStatus(this.userId, this.status);
      });
  }
}
