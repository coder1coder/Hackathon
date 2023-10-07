import {AfterViewChecked, Component, ElementRef, OnInit, ViewChild} from "@angular/core";
import {UserRoleTranslator} from "src/app/models/User/UserRole";
import {AuthService} from "../../../services/auth.service";
import {TeamClient} from "../../../services/team-client.service";
import {catchError, of, switchMap, takeUntil} from "rxjs";
import {IUpdateUser, IUser} from "../../../models/User/IUser";
import {UserProfileReaction} from "src/app/models/User/UserProfileReaction";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../../services/user.service";
import {UserProfileReactionService} from "../../../services/user-profile-reaction.service";
import {SnackService} from "../../../services/snack.service";
import {FriendshipStatus} from "../../../models/Friendship/FriendshipStatus";
import {MatTabGroup} from "@angular/material/tabs";
import {Team} from "../../../models/Team/Team";
import {UserEmailStatus} from "src/app/models/User/UserEmailStatus";
import {WithFormComponentBase} from "../../WithFormComponentBase";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {IKeyValue} from "../../../common/key-value.interface";
import {emailRegex} from "../../../common/patterns/email-regex";

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls:['./profile.view.component.scss']
})
export class ProfileViewComponent extends WithFormComponentBase implements OnInit, AfterViewChecked {

  @ViewChild(MatTabGroup) public friendshipTabs!: MatTabGroup;
  @ViewChild('confirmationCodeInput') confirmationCodeInput: ElementRef;

  public form = new FormGroup({});
  public UserRoleTranslator = UserRoleTranslator;
  public userId: number;
  public user: IUser;
  public userTeam: Team | null;
  public authUserId: number;
  public isEditMode: boolean = false;
  public isLoading: boolean = true;
  public userProfileReaction = UserProfileReaction;
  public friendshipStatus = FriendshipStatus;
  public userEmailStatus = UserEmailStatus;

  public canUploadImage: boolean = false;
  public canViewEmail: boolean = false;

  private userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  private emailRegexp: RegExp = emailRegex;

  constructor(
    private authService: AuthService,
    private teamService: TeamClient,
    private activateRoute: ActivatedRoute,
    private userService: UserService,
    private userProfileReactionService: UserProfileReactionService,
    private snackService: SnackService,
    private fb: FormBuilder
  ) {
    super();
    this.userService = userService;
    this.snackService = snackService;
    this.authUserId = this.authService.getUserId() ?? 0;
    this.userId = this.activateRoute.snapshot.params['userId'] ?? this.authUserId;
  }

  ngOnInit(): void {
    this.fetchData();
    this.initForm();
  }

  public ngAfterViewChecked(): void {
    if (this.userId == this.authUserId) {
      setTimeout(() =>{
        this.friendshipTabs?.realignInkBar()
      }, 0);
    }
  }

  public userEdit(): void {
    this.isEditMode = true;
    if (Boolean(this.user)) {
      this.form.setValue(this.mapUserValue(this.user));
    }
  }

  public saveUserEdit(): void {
    const request: IUpdateUser = {
      id: this.userId,
      ...this.form.getRawValue()
    };
    if (JSON.stringify(request) !== JSON.stringify(this.mapUserValue(this.user))) {
      this.userService.updateUser(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.isEditMode = false;
            this.snackService.open("Данные профиля изменены");
            this.fetchData();
          },
          error: (error) => {
            const errorDetail = error?.error?.detail;
            if (errorDetail) {
              this.snackService.open(errorDetail);
            } else {
              this.snackService.open("Не удалось обновить профиль");
            }
          }
        });
    } else {
      this.isEditMode = false;
    }
  }

  public userEditCancel(): void {
    this.isEditMode = false;
    this.form.reset();
  }

  public toggleReaction(event: Event, reaction: UserProfileReaction):void {
    this.userProfileReactionService.toggleReaction(this.userId, reaction)
      .pipe(takeUntil(this.destroy$))
      .subscribe((_)=>{
        this.fetchReactions();
      });
  }

  public getAvailableUserProfileReactions(): number[] {
    return Object.keys(UserProfileReaction)
      .filter(x=>!isNaN(Number(x)))
      .map(s=>Number(s))
      .filter(x=>x !== UserProfileReaction.None);
  }

  public isReactionActive(reaction: UserProfileReaction): boolean {
    return (this.userProfileReactions & reaction) === reaction;
  }

  public confirmEmail(): void {
    const code = this.confirmationCodeInput?.nativeElement?.value ?? '';
    this.userService.confirmEmail(code)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchData(),
        error: () => this.snackService.open("Ошибка подтверждения Email")
      })
  }

  public createEmailConfirmationRequest() {
    this.userService.createEmailConfirmationRequest()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchData(),
        error: () => this.snackService.open("Ошибка отправки запроса на подтверждение Email")
      });
  }

  private fetchReactions(): void {
    this.userProfileReactionService.get(this.userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (r: UserProfileReaction) => {
          this.userProfileReactions = r;
        },
        error: () => {}
      });
  }

  private fetchData(): void {
    this.isLoading = true;
    this.userService.getById(this.userId)
      ?.pipe(
        takeUntil(this.destroy$),
        switchMap((user)=> {
          const currentUserId = this.authService.getUserId();
          this.canUploadImage = currentUserId === this.userId;
          this.canViewEmail = currentUserId === this.userId;
          this.user = user;

          if (this.userId != this.authUserId) {
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

  private initForm(): void {
    this.form = this.fb.group({
      fullName: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: [null, [Validators.required, Validators.pattern(this.emailRegexp)]]
    })
  }

  private mapUserValue(user: IUser): IKeyValue {
    return {
      fullName: user.fullName ? user.fullName : null,
      email: user.email?.address ? user.email.address : null,
    }
  }
}
