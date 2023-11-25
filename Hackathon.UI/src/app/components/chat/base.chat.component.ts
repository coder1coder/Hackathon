import { AuthService } from "../../services/auth.service";
import { ElementRef, HostListener, Injectable, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from "@angular/forms";
import {
  BehaviorSubject,
  bufferTime,
  filter,
  from,
  mergeMap,
  Subject,
  takeUntil,
} from "rxjs";
import { TABLE_DATE_FORMAT } from "../../common/consts/date-formats";
import { WithFormBaseComponent } from "../../common/base-components/with-form-base.component";
import { ProfileUserStore } from "../../shared/stores/profile-user.store";
import { IUser } from "../../models/User/IUser";
import { PaginationSorting } from "../../models/GetListParameters";
import { PageSettingsDefaults } from "../../models/PageSettings";

@Injectable()
export abstract class BaseChatComponent<TChatMessage>
  extends WithFormBaseComponent implements OnInit, OnDestroy {

  @ViewChild('formComponent') formComponent: NgForm;

  public currentUserId: number;
  public isFloatMode: boolean = false;
  public form: FormGroup = this.fb.group({});
  public tableDateFormat = TABLE_DATE_FORMAT;
  public isLoaded: boolean = false;
  public isNeedReloadData: boolean = true;
  public chatMembers: Map<number, IUser> = new Map<number, IUser>();
  public params = new PaginationSorting();
  public selectedPageIndex: BehaviorSubject<number> = new BehaviorSubject<number>(-1);
  public isScrollLoad: boolean = true;

  protected abstract messages: TChatMessage[];
  protected abstract chatBody: ElementRef;
  protected destroy$ = new Subject();

  private totalMessages: number = 0;
  private minLength: number = 2;
  private maxLength: number = 200;
  private noExistedMembersChanges = new Subject<number>();
  private noExistedMemberIds: number[] = [];
  private scrollPositionBeforeLoad: number = 0;
  private scrollPositionInPercent: number = 0;
  private readonly maxScrollPercentageChatContainer: number = 80;
  private readonly distanceFromBottomChatContainerInPercent: number = 20;

  protected constructor(
    protected authService: AuthService,
    protected fb: FormBuilder,
    protected profileUserStore: ProfileUserStore,
  ) {
    super();
  }

  @HostListener('document:keydown.enter', ['$event'])
  public onPushEnter(): void {
    this.scrollChatToLastMessage();
  }

  public onScroll(): void {
    const element = this.chatBody?.nativeElement;
    if (element) {
      const scrollPosition = element.scrollTop;
      const scrollHeight = element.scrollHeight;
      const clientHeight = element.clientHeight;
      const scrollPercentage = (scrollPosition / (scrollHeight - clientHeight)) * 100;
      this.scrollPositionBeforeLoad = scrollPosition;
      this.scrollPositionInPercent = Math.round(Math.abs(scrollPercentage));
      if (this.scrollPositionInPercent >= this.maxScrollPercentageChatContainer && !this.isScrollLoad && this.isCanLoad) {
        this.isScrollLoad = true;
        this.fetchMessages();
      }
    }
  }

  public ngOnInit(): void {
    this.initForm();
    this.initSubscriptions();
  }

  public ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
    this.chatMembers.clear();
    this.isLoaded = false;
    this.isScrollLoad = false;
  }

  public changeParamsAfterLoading(totalMessages: number): void {
    this.params.Offset += PageSettingsDefaults.Limit;
    this.isScrollLoad = false;
    this.totalMessages = totalMessages ?? 0;
    if (this.chatBody?.nativeElement) {
      this.chatBody.nativeElement.scrollTop = this.scrollPositionBeforeLoad;
    }
  }

  public get isCanLoad(): boolean {
    return this.messages?.length !== this.totalMessages;
  }

  public get canView(): boolean {
    return this.authService.isLoggedIn() && this.entityId.getValue() > 0;
  }

  public get isUserNearBottom(): boolean {
    return this.distanceFromBottomChatContainerInPercent >= this.scrollPositionInPercent;
  }

  public abstract get members(): IUser[];
  public abstract get canSendMessageWithNotify(): boolean;
  public abstract entityId: BehaviorSubject<number>;
  public abstract fetchEntity(needReload?: boolean): void;
  public abstract fetchMessages(): void;
  public abstract sendMessage(): void;

  public loadChatUsers(): void {
    this.isLoaded = false;
    this.chatMembers.clear();
    from(this.members)
      .pipe(
        mergeMap((user: IUser) => this.profileUserStore.getUser(user.id)),
        takeUntil(this.destroy$),
      ).subscribe({
        next: (user: IUser) => this.chatMembers.set(user.id, user),
        complete: () => {
          this.isLoaded = true;
        },
      });
  }

  public initForm(): void{
    this.form = this.fb.group({
      message: new FormControl('',[
        Validators.minLength(this.minLength),
        Validators.maxLength(this.maxLength),
      ]),
      notify: new FormControl(false, [
        Validators.required,
      ]),
    });
  }

  public getErrorLengthMessage(control: FormControl): string {
    if (control.hasError('minlength')) {
      return `Минимальная длина ${this.minLength} символов`;
    } else if (control.hasError('maxlength')) {
      return `Максимальная длина ${this.maxLength} символов`;
    } else {
      return '';
    }
  }

  public scrollChatToLastMessage(): void {
    setTimeout(() => {
      if (this.chatBody?.nativeElement) {
        this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
      }
    }, 100);
  }

  public onElementsChanged(isNearBottom: boolean, forceScroll: boolean = false): void {
    if (forceScroll || isNearBottom) {
      this.scrollChatToLastMessage();
    }
  }

  public getMember(id: number): IUser {
    if (
      !this.chatMembers.get(id) &&
      id !== this.currentUserId &&
      !this.noExistedMemberIds.includes(id)
    ) {
      this.noExistedMemberIds.push(id);
      this.noExistedMembersChanges.next(id);
    }

    return this.chatMembers.get(id);
  }

  private initSubscriptions(): void {
    this.entityId
      .pipe(takeUntil(this.destroy$))
      .subscribe((value: number) => {
        if (value < 1) return;
        this.fetchEntity(this.isNeedReloadData);
        this.fetchMessages();
      });

    this.currentUserId = this.authService.getUserId() ?? -1;
    this.authService.authChange
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchEntity(this.isNeedReloadData);
        this.fetchMessages();
      });

    this.noExistedMembersChanges
      .pipe(
        bufferTime(300),
        filter(values => values.length > 0),
        mergeMap(from),
        mergeMap((id: number) => this.profileUserStore.getUser(id)),
        takeUntil(this.destroy$),
      ).subscribe({
        next: (user: IUser) => this.chatMembers.set(user.id, user),
      });

    this.selectedPageIndex
      .pipe(
        filter((v) => v !== -1),
        takeUntil(this.destroy$),
      )
      .subscribe(() => this.scrollChatToLastMessage());
  }
}
