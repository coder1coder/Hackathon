import {AfterViewInit, Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../../services/user.service";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {SnackService} from "../../../services/snack.service";
import {IUser} from "../../../models/User/IUser";
import {UserProfileReaction} from "../../../models/User/UserProfileReaction";
import {UserProfileReactionService} from "../../../services/user-profile-reaction.service";

@Component({
  selector: 'user-view',
  templateUrl: './user.view.component.html',
  styleUrls: ['./user.view.component.scss']
})
export class UserViewComponent implements AfterViewInit {

  userId: number;
  user?: IUser;
  isLoading: boolean = true;

  userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  UserProfileReaction = UserProfileReaction;
  Object = Object;

  availableReactions: string[] = [];

  constructor(
    private activateRoute: ActivatedRoute,
    private usersService: UserService,
    private userProfileReactionService: UserProfileReactionService,
    private authService: AuthService,
    private snackBar: SnackService,
  ) {
    this.userId = activateRoute.snapshot.params['userId'];
    this.usersService = usersService;
    this.snackBar = snackBar;
  }

  ngAfterViewInit(): void {

    this.fetchUser();
    this.fetchReactions();
  }

  public toggleReaction(event: Event, reaction: UserProfileReaction):void {
    this.userProfileReactionService.toggleReaction(this.userId, reaction)
      .subscribe((r)=>{
        this.fetchReactions();
      });
  }

  public getAvailableUserProfileReactions():number[]
  {
    return Object.keys(UserProfileReaction)
      .filter(x=>!isNaN(Number(x)))
      .map(s=>Number(s))
      .filter(x=>x !== UserProfileReaction.None);
  }

  public isReactionActive(reaction: UserProfileReaction):boolean {
    return (this.userProfileReactions & reaction) === reaction;
  }

  private fetchUser() {
    this.isLoading = true;
    this.usersService.getById(this.userId)
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: (r: IUser) => {
          this.user = r;
        },
        error: () => {
        }
      });
  }

  private fetchReactions() {
    this.userProfileReactionService.get(this.userId)
      .subscribe({
        next: (r: UserProfileReaction) => {
          this.userProfileReactions = r;
        },
        error: () => {
          console.log('not found reaction')
        }
      });
  }
}
