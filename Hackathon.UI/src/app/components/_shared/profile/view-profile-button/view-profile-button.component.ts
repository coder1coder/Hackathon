import {Component, Input} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {RouterService} from "../../../../services/router.service";

@Component({
  selector: `view-profile-button`,
  template: `<button mat-button mat-stroked-button (click)="this.routerService.Profile.View(this.userId)">Посмотреть профиль</button>`
})
export class ViewProfileButtonComponent
{
  @Input()
  set userId(value) { this._userId.next(value); };
  get userId() { return this._userId.getValue(); }
  private _userId = new BehaviorSubject<number>(0);

  constructor(
    public routerService: RouterService
    ) {}

}
