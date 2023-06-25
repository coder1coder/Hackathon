import {AfterViewInit, Component, Input, OnInit} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {AuthService} from "../../../services/auth.service";
import {FriendshipService} from "../../../services/friendship/friendship.service";
import {GetListParameters} from "../../../models/GetListParameters";
import {FriendshipFilter} from "../../../models/Friendship/FriendshipFilter";
import {FriendshipStatus, GetOfferOption, IFriendship} from "../../../models/Friendship/FriendshipStatus";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: `view-profile-button`,
  template: `<button mat-button mat-stroked-button (click)="doAction()">Посмотреть профиль</button>`
})
export class ViewProfileButtonComponent
{
  @Input()
  set userId(value) { this._userId.next(value); };
  get userId() { return this._userId.getValue(); }
  private _userId = new BehaviorSubject<number>(0);

  constructor(
    private routerService: RouterService
    ) {

  }

  doAction(){
    this.routerService.Profile.View(this.userId);
  }

}
