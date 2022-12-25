import {AfterViewChecked, AfterViewInit, Component, ElementRef, ViewChild} from "@angular/core";
import {UserRoleTranslator} from "src/app/models/User/UserRole";
import {AuthService} from "../../../services/auth.service";
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
import {Team} from "../../../models/Team/Team";
import { UserEmailStatus } from "src/app/models/User/UserEmailStatus";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent implements AfterViewInit, AfterViewChecked {

  UserRoleTranslator = UserRoleTranslator;
  @ViewChild(MatTabGroup) public friendshipTabs!: MatTabGroup;
  @ViewChild('confirmationCodeInput') confirmationCodeInput: ElementRef;

  userId: number;
  user?: IUser;
  userTeam?: Team | null;
  authUserId: number;

  canUploadImage: boolean = false;

  isLoading: boolean = true;

  userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  UserProfileReaction = UserProfileReaction;
  FriendshipStatus = FriendshipStatus;
  UserEmailStatus = UserEmailStatus;

  Object = Object;

  availableReactions: string[] = [];

  constructor(
    private authService: AuthService,
    private teamService: TeamService,
    private activateRoute: ActivatedRoute,
    private userService: UserService,
    private userProfileReactionService: UserProfileReactionService,
    private snackService: SnackService,
  ) {

    this.userService = userService;
    this.snackService = snackService;

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
    this.fetchData();
  }

  private fetchData(): void{

    this.isLoading = true;
    this.userService.getById(this.userId)
      ?.pipe(
        switchMap((user)=> {

          let currentUserId = this.authService.getUserId();

          this.canUploadImage = currentUserId == this.userId;

          this.user = user;

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
        this.userTeam = res
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
        error: _ => {
        }
      });
  }

  public confirmEmail():void{
    let code = this.confirmationCodeInput?.nativeElement?.value ?? '';
    this.userService.confirmEmail(code)
      .subscribe({
        next: () => {
          this.fetchData();
        },
        error: _ => {
          this.snackService.open("Ошибка подтверждения Email")
        }
      })
  }

  public createEmailConfirmationRequest()
  {
    this.userService.createEmailConfirmationRequest()
      .subscribe({
        next: () =>{
          this.fetchData();
        },
        error: _ => {
          this.snackService.open("Ошибка отправки запроса на подтверждение Email");
        }
      });
  }

}
