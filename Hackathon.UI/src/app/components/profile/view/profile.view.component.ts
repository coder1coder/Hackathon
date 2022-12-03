import {AfterViewInit, Component, OnInit} from "@angular/core";
import {UserRoleTranslator} from "src/app/models/User/UserRole";
import {AuthService} from "../../../services/auth.service";
import {KeyValue} from "@angular/common";
import {TeamService} from "../../../services/team.service";
import {catchError, Observable, of, switchMap} from "rxjs";
import {IUser} from "../../../models/User/IUser";
import {UserProfileReaction} from "src/app/models/User/UserProfileReaction";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../../services/user.service";
import {UserProfileReactionService} from "../../../services/user-profile-reaction.service";
import {SnackService} from "../../../services/snack.service";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit {

  UserRoleTranslator = UserRoleTranslator;

  public userProfileDetails: KeyValue<string,any>[] = [];

  userId: number;
  user?: IUser;

  canUploadImage: boolean = false;

  isLoading: boolean = true;

  userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  UserProfileReaction = UserProfileReaction;
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

    this.userId = this.activateRoute.snapshot.params['userId'] ?? this.authService.getUserId();
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

        this.fetchReactions();

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

  private fetchUser(): Observable<IUser> | null {
    return this.usersService.getById(this.userId);
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
