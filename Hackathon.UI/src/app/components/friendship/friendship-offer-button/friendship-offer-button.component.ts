import {AfterViewInit, Component, Input, OnInit} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {AuthService} from "../../../services/auth.service";
import {FriendshipService} from "../../../services/friendship/friendship.service";
import {GetListParameters} from "../../../models/GetListParameters";
import {FriendshipFilter} from "../../../models/Friendship/FriendshipFilter";
import {FriendshipStatus, GetOfferOption, IFriendship} from "../../../models/Friendship/FriendshipStatus";
import {SignalRService} from "../../../services/signalr.service";

@Component({
  selector: `friendship-offer-button`,
  templateUrl: `./friendship-offer-button.component.html`
})
export class FriendshipOfferButtonComponent implements OnInit,  AfterViewInit
{
  @Input()
  set friendId(value) { this._friendId.next(value); };
  get friendId() { return this._friendId.getValue(); }
  private _friendId = new BehaviorSubject<number>(0);

  public authUserId: number | undefined;

  public isEnabled: boolean = false;

  public statusText: string = 'Нельзя определить статус';
  public friendship?: IFriendship;

  constructor(
    private authService: AuthService,
    private friendshipService: FriendshipService,
    private signalRService: SignalRService,
    ) {

  }

  ngOnInit(): void {

    this.authUserId = this.authService.getUserId();

    this._friendId.subscribe(friendId => {
      if (friendId !== this.authUserId)
        this.isEnabled = true;
    });

  }

  ngAfterViewInit() {

    this.signalRService.onFriendshipChangedIntegration = x => {

      if (this.authUserId !== undefined
        && (x.userIds.indexOf(this.authUserId) >= 0 || x.userIds.indexOf(this.friendId) >= 0))
        this.resolveStatus();

    }

    this.resolveStatus();
  }

  private resolveStatus(){
    let parameters = new GetListParameters<FriendshipFilter>();
    parameters.Filter = new FriendshipFilter();
    parameters.Filter.option = GetOfferOption.Any;

    this.friendshipService.getOffers(parameters)
      .subscribe(x => {

        this.friendship = x.items[0];

        if (this.friendship != undefined)
        {
          this.friendship = x.items[0];

          switch (this.friendship.status)
          {
            case FriendshipStatus.Pending:

              this.statusText = (this.friendship.proposerId == this.authUserId)
              ? `Отписаться от пользователя`
              : `Принять запрос дружбы`;

              break;

            case FriendshipStatus.Confirmed:
              this.statusText = `Перестать дружить`
              break;

            case FriendshipStatus.Rejected:
              this.statusText = `Подать заявку в друзья`
              break;
          }
        } else this.statusText = `Подать заявку в друзья`
      });
  }

  doAction(){

    if (this.friendship !== undefined  )
    {
      switch (this.friendship.status)
      {
        case FriendshipStatus.Pending:

          if (this.friendship.proposerId == this.authUserId)
          {
            this.friendshipService.unsubscribe(this.friendship.userId)
              .subscribe(_=> this.resolveStatus());
          } else {
            this.friendshipService.createOrAcceptOffer(this.friendship.proposerId)
              .subscribe(_=> this.resolveStatus());
          }

          break;

        case FriendshipStatus.Confirmed:
          this.friendshipService.endFriendship(this.friendId)
            .subscribe(_=> this.resolveStatus());
          break;

        case FriendshipStatus.Rejected:
          this.sendFriendshipRequest();
          break;
      }
    } else this.sendFriendshipRequest();
  }

  private sendFriendshipRequest()
  {
    this.friendshipService.createOrAcceptOffer(this.friendId)
      .subscribe(_=> this.resolveStatus());
  }

}
