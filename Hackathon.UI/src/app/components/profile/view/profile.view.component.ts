import { AfterViewChecked, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UserRoleTranslator } from 'src/app/models/User/UserRole';
import { AuthService } from '../../../services/auth.service';
import { TeamClient } from '../../../services/team-client.service';
import { catchError, Observable, of, switchMap, takeUntil } from 'rxjs';
import { IUpdateUser, IUser } from '../../../models/User/IUser';
import { UserProfileReaction, IUserProfileReaction } from 'src/app/models/User/UserProfileReaction';
import { ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { UserProfileReactionService } from '../../../services/user-profile-reaction.service';
import { SnackService } from '../../../services/snack.service';
import { FriendshipStatus } from '../../../models/Friendship/FriendshipStatus';
import { MatTabGroup } from '@angular/material/tabs';
import { Team } from '../../../models/Team/Team';
import { UserEmailStatus } from 'src/app/models/User/UserEmailStatus';
import { WithFormBaseComponent } from '../../../common/base-components/with-form-base.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { IKeyValue } from '../../../common/interfaces/key-value.interface';
import { emailRegex } from '../../../common/patterns/email-regex';
import { checkValue } from '../../../common/functions/check-value';
import { ProfileUserStore } from '../../../shared/stores/profile-user.store';
import { fromMobx } from '../../../common/functions/from-mobx.function';
import { CurrentUserStore } from '../../../shared/stores/current-user.store';
import { AppStateService } from '../../../services/app-state.service';
import { finalize } from 'rxjs/operators';

@Component({
  templateUrl: './profile.view.component.html',
  styleUrls: ['./profile.view.component.scss'],
})
export class ProfileViewComponent
  extends WithFormBaseComponent
  implements OnInit, AfterViewChecked
{
  @ViewChild(MatTabGroup) public friendshipTabs: MatTabGroup;
  @ViewChild('confirmationCodeInput') confirmationCodeInput: ElementRef;

  public form = new FormGroup({});
  public UserRoleTranslator = UserRoleTranslator;
  public userId: number;
  public user: IUser;
  public currentUser: IUser;
  public userTeam: Team;
  public authUserId: number;
  public isEditMode: boolean = false;
  public canUploadImage: boolean = false;
  public canViewEmail: boolean = false;
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);
  public userProfileReaction = UserProfileReaction;
  public friendshipStatus = FriendshipStatus;
  public userEmailStatus = UserEmailStatus;
  public userProfileReactionsList: IUserProfileReaction[] = [];

  private userProfileReactions: UserProfileReaction = UserProfileReaction.None;
  private emailRegexp: RegExp = emailRegex;

  constructor(
    private authService: AuthService,
    private teamService: TeamClient,
    private activateRoute: ActivatedRoute,
    private userService: UserService,
    private userProfileReactionService: UserProfileReactionService,
    private snackService: SnackService,
    private fb: FormBuilder,
    private profileUserStore: ProfileUserStore,
    private currentUserStore: CurrentUserStore,
    private appStateService: AppStateService,
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
    if (this.userId === this.authUserId) {
      setTimeout(() => {
        this.friendshipTabs?.realignInkBar();
      }, 0);
    }
  }

  public get isOwner(): boolean {
    return this.user?.id === this.currentUser?.id;
  }

  public userEdit(): void {
    this.isEditMode = true;
    if (this.user) {
      this.form.setValue(this.mapUserValue(this.user));
    }
  }

  public saveUserEdit(): void {
    const request: IUpdateUser = {
      id: this.userId,
      ...this.form.getRawValue(),
    };
    if (JSON.stringify(request) !== JSON.stringify(this.mapUserValue(this.user))) {
      this.userService
        .updateUser(request)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.isEditMode = false;
            this.snackService.open('Данные профиля изменены');
            this.fetchData(true);
          },
          error: (error) => {
            const errorDetail: string = error?.error?.detail;
            if (errorDetail) {
              this.snackService.open(errorDetail);
            } else {
              this.snackService.open('Не удалось обновить профиль');
            }
          },
        });
    } else {
      this.isEditMode = false;
    }
  }

  public userEditCancel(): void {
    this.isEditMode = false;
    this.form.reset();
  }

  public toggleReaction(event: Event, reaction: UserProfileReaction): void {
    this.userProfileReactionService
      .toggleReaction(this.userId, reaction)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchReactions();
        this.fetchReactionsCount();
      });
  }

  public getAvailableUserProfileReactions(): number[] {
    return Object.keys(UserProfileReaction)
      .filter((x) => !isNaN(Number(x)))
      .map((s) => Number(s))
      .filter((x) => x !== UserProfileReaction.None);
  }

  public isReactionActive(reaction: UserProfileReaction): boolean {
    return (this.userProfileReactions & reaction) === reaction;
  }

  public confirmEmail(): void {
    const code: string = this.confirmationCodeInput?.nativeElement?.value ?? '';
    this.userService
      .confirmEmail(code)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchData(),
        error: () => this.snackService.open('Ошибка подтверждения Email'),
      });
  }

  public createEmailConfirmationRequest(): void {
    this.userService
      .createEmailConfirmationRequest()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => this.fetchData(),
        error: () => this.snackService.open('Ошибка отправки запроса на подтверждение Email'),
      });
  }

  private fetchData(needReload: boolean = false): void {
    this.appStateService.setIsLoadingState(true);
    this.profileUserStore
      .getUser(this.userId, needReload)
      .pipe(
        switchMap((user: IUser) => {
          const currentUserId: number = this.authService.getUserId();
          this.canUploadImage = currentUserId === this.userId;
          this.canViewEmail = currentUserId === this.userId;
          this.user = user;

          if (this.userId !== this.authUserId) {
            this.fetchReactions();
          }
          this.fetchReactionsCount();
          return this.teamService.getMyTeam();
        }),
        catchError(() => of(null)),
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe((res: Team) => (this.userTeam = res));

    fromMobx(() => this.currentUserStore.currentUser)
      .pipe(takeUntil(this.destroy$))
      .subscribe((user: IUser) => (this.currentUser = user));
  }

  private fetchReactions(): void {
    this.userProfileReactionService
      .get(this.userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (reaction: UserProfileReaction) => {
          this.userProfileReactions = reaction;
        },
      });
  }

  private fetchReactionsCount(): void {
    this.userProfileReactionService
      .getCount(this.isOwner ? this.authUserId : this.userId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (reactionModelList: IUserProfileReaction[]) => {
          this.userProfileReactionsList = reactionModelList;
        },
      });
  }

  private initForm(): void {
    this.form = this.fb.group({
      fullName: [null, [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      email: [null, [Validators.required, Validators.pattern(this.emailRegexp)]],
    });
  }

  private mapUserValue(user: IUser): IKeyValue {
    return {
      fullName: checkValue(user?.fullName),
      email: checkValue(user?.email?.address),
    };
  }
}
