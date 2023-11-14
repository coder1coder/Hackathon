import {Component, Input, OnInit} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {FriendshipService} from "../../../services/friendship/friendship.service";
import {FriendshipStatus} from "../../../models/Friendship/FriendshipStatus";
import {BaseCollection} from "../../../models/BaseCollection";
import {IUser} from "../../../models/User/IUser";
import {RouterService} from "../../../services/router.service";
import {AuthService} from "../../../services/auth.service";
import {SignalRService} from "../../../services/signalr.service";

@Component({
  selector: `friends-list`,
  styleUrls: ['./friends-list.component.scss'],
  templateUrl: `./friends-list.component.html`
})
export class FriendsListComponent implements OnInit
{
  @Input()
  set userId(value) { this._userId.next(value); };
  get userId() { return this._userId.getValue(); }
  private _userId = new BehaviorSubject<number>(0);

  @Input()
  status: FriendshipStatus | undefined;

  FriendshipStatus = FriendshipStatus;

  Number = Number;

  users: IUser[] = [];

  public authUserId: number | undefined;

  constructor(
    private friendshipService: FriendshipService,
    private routerService: RouterService,
    private authService: AuthService,
    private signalRService: SignalRService,
  ) {
  }

  ngOnInit(): void {

    this._userId.subscribe(x=>{
      this.fetchUsersByStatus(x, this.status);
    })

    this.authUserId = this.authService.getUserId();

    this.signalRService.onFriendshipChangedIntegration = x => {

      if (this.authUserId !== undefined
        && (x.userIds.indexOf(this.authUserId) >= 0 || x.userIds.indexOf(this.userId) >= 0))
        this.fetchUsersByStatus(this.userId, this.status);

    }

  }

  private fetchUsersByStatus(userId:number, status?: FriendshipStatus){
    if (status != undefined)
      this.friendshipService.getUsersByFriendshipStatus(userId, status)
        .subscribe((x:BaseCollection<IUser>)=>{
          this.users = x.items
        })
  }

  getNoDataText():string{
    switch (this.status)
    {
      case FriendshipStatus.Pending: return `Заявки в друзья не найдены`;
      case FriendshipStatus.Confirmed: return `У вас еще нет друзей`;
      default: return `Нет данных`;
    }
  }

  public openUserProfile(userId:number)
  {
    this.routerService.Users.View(userId);
  }

  public rejectOffer(userId:number)
  {
    this.friendshipService.rejectOffer(userId)
      .subscribe(_=> {
        this.fetchUsersByStatus(this.userId, this.status);
      })
  }

  public acceptOffer(userId:number)
  {
    this.friendshipService.createOrAcceptOffer(userId)
      .subscribe(_=> {
        this.fetchUsersByStatus(this.userId, this.status);
      })
  }

  public removeFromFriends(userId:number)
  {
    this.friendshipService.endFriendship(userId)
      .subscribe(_=> {
        this.fetchUsersByStatus(this.userId, this.status);
      })
  }


}
