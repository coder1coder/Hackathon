import {AfterViewChecked, AfterViewInit, Component, ViewChild} from "@angular/core";
import {UserRoleTranslator} from "src/app/models/User/UserRole";
import {AuthService} from "../../../services/auth.service";
import {KeyValue} from "@angular/common";
import {TeamService} from "../../../services/team.service";
import {catchError, of, switchMap} from "rxjs";
import {IUser} from "../../../models/User/IUser";
import {UserProfileReaction} from "src/app/models/User/UserProfileReaction";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../../services/user.service";
import {UserProfileReactionService} from "../../../services/user-profile-reaction.service";
import {SnackService} from "../../../services/snack.service";
import {FriendshipStatus} from "../../../models/Friendship/FriendshipStatus";
import {MatTabGroup} from "@angular/material/tabs";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit, AfterViewChecked {

  UserRoleTranslator = UserRoleTranslator;
  @ViewChild(MatTabGroup) public friendshipTabs!: MatTabGroup;

  public userProfileDetails: KeyValue<string,any>[] = [];

  userId: number;
  user?: IUser;

  authUserId: number;

  canUploadImage: boolean = false;

  isLoading: boolean = true;

  userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  UserProfileReaction = UserProfileReaction;
  FriendshipStatus = FriendshipStatus;
  Object = Object;

  availableReactions: string[] = [];

  constructor(
    private authService: AuthService,
    private teamService: TeamService,
    private activateRoute: ActivatedRoute,
    private usersService: UserService,
    private userProfileReactionService: UserProfileReactionService,
    private snackBar: SnackService,
  ) {

    this.usersService = usersService;
    this.snackBar = snackBar;

    this.authUserId = this.authService.getUserId() ?? 0;
    this.userId = this.activateRoute.snapshot.params['userId'] ?? this.authUserId;
  }

  public ngAfterViewChecked(): void {

    if (this.userId == this.authUserId)
    {
      setTimeout(() =>{
        this.friendshipTabs?.realignInkBar()
      }, 0);
    }

  }

  ngAfterViewInit(): void {



    this.isLoading = true;

    this.usersService.getById(this.userId)
    ?.pipe(
      switchMap((user)=> {

        let currentUserId = this.authService.getUserId();

        this.canUploadImage = currentUserId == this.userId;

        this.user = user;

        this.userProfileDetails = [
          { key: 'Имя пользователя', value: user?.userName},
          { key: 'Полное имя', value: user?.fullName},
          { key: 'E-mail', value: user?.email!},
          { key: 'Роль', value: user?.role!.toString()},
        ]

        if (this.userId != this.authUserId)
        {
          this.fetchReactions();
        }

        return this.teamService.getMyTeam()
      }),
      catchError(() => {
        return of(null)
      })
    ).subscribe((res) => {
        this.isLoading = false;
        this.userProfileDetails.push({ key: 'Команда', value: res?.name ?? "Не состоит в команде" })
      }
    )
  }

  public toggleReaction(event: Event, reaction: UserProfileReaction):void {
    this.userProfileReactionService.toggleReaction(this.userId, reaction)
      .subscribe((_)=>{
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
