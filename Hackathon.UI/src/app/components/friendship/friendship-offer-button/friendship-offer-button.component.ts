import { AfterViewInit, Component, Input, OnDestroy, OnInit } from '@angular/core';
import { BehaviorSubject, Subject, takeUntil } from 'rxjs';
import { AuthService } from '../../../services/auth.service';
import { GetListParameters } from '../../../models/GetListParameters';
import { FriendshipFilter } from '../../../models/Friendship/FriendshipFilter';
import {
  FriendshipStatus,
  GetOfferOption,
  IFriendship,
} from '../../../models/Friendship/FriendshipStatus';
import { SignalRService } from '../../../services/signalr.service';
import { BaseCollection } from '../../../models/BaseCollection';
import { FriendshipClient } from 'src/app/clients/friendship.client';

@Component({
  selector: `friendship-offer-button`,
  templateUrl: `./friendship-offer-button.component.html`,
})
export class FriendshipOfferButtonComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input()
  set friendId(value) {
    this._friendId.next(value);
  }
  get friendId(): number {
    return this._friendId.getValue();
  }

  public authUserId: number;
  public isEnabled: boolean = false;
  public statusText: string = 'Нельзя определить статус';
  public friendship: IFriendship;

  private _friendId = new BehaviorSubject<number>(0);
  private destroy$ = new Subject();

  constructor(
    private authService: AuthService,
    private friendshipClient: FriendshipClient,
    private signalRService: SignalRService,
  ) {}

  ngOnInit(): void {
    this.authUserId = this.authService.getUserId();

    this._friendId.pipe(takeUntil(this.destroy$)).subscribe((friendId) => {
      if (friendId !== this.authUserId) this.isEnabled = true;
    });
  }

  ngAfterViewInit(): void {
    this.signalRService.onFriendshipChangedIntegration = (x): void => {
      if (
        this.authUserId !== undefined &&
        (x.userIds.indexOf(this.authUserId) >= 0 || x.userIds.indexOf(this.friendId) >= 0)
      )
        this.resolveStatus();
    };

    this.resolveStatus();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private resolveStatus(): void {
    const parameters: GetListParameters<FriendshipFilter> =
      new GetListParameters<FriendshipFilter>();
    parameters.Filter = new FriendshipFilter();
    parameters.Filter.option = GetOfferOption.Any;

    this.friendshipClient
      .getOffers(parameters)
      .pipe(takeUntil(this.destroy$))
      .subscribe((x: BaseCollection<IFriendship>) => {
        this.friendship = x.items[0];

        if (this.friendship != undefined) {
          this.friendship = x.items[0];

          switch (this.friendship.status) {
            case FriendshipStatus.Pending:
              this.statusText =
                this.friendship.proposerId == this.authUserId
                  ? `Отписаться от пользователя`
                  : `Принять запрос дружбы`;

              break;

            case FriendshipStatus.Confirmed:
              this.statusText = `Перестать дружить`;
              break;

            case FriendshipStatus.Rejected:
              this.statusText = `Подать заявку в друзья`;
              break;
          }
        } else this.statusText = `Подать заявку в друзья`;
      });
  }

  public doAction(): void {
    if (this.friendship !== undefined) {
      switch (this.friendship.status) {
        case FriendshipStatus.Pending:
          if (this.friendship.proposerId == this.authUserId) {
            this.friendshipClient
              .unsubscribe(this.friendship.userId)
              .pipe(takeUntil(this.destroy$))
              .subscribe(() => this.resolveStatus());
          } else {
            this.friendshipClient
              .createOrAcceptOffer(this.friendship.proposerId)
              .pipe(takeUntil(this.destroy$))
              .subscribe(() => this.resolveStatus());
          }

          break;

        case FriendshipStatus.Confirmed:
          this.friendshipClient
            .endFriendship(this.friendId)
            .pipe(takeUntil(this.destroy$))
            .subscribe(() => this.resolveStatus());
          break;

        case FriendshipStatus.Rejected:
          this.sendFriendshipRequest();
          break;
      }
    } else this.sendFriendshipRequest();
  }

  private sendFriendshipRequest(): void {
    this.friendshipClient
      .createOrAcceptOffer(this.friendId)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.resolveStatus());
  }
}
