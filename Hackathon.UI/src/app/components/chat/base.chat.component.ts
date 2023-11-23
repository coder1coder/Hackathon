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

@Injectable()
export abstract class BaseChatComponent<TChatMessage>
  extends WithFormBaseComponent implements OnInit, OnDestroy {

  @ViewChild('formComponent') formComponent: NgForm;

  public currentUserId: number;
  public isOpened: boolean = false
  public isFloatMode: boolean = false;
  public isNearBottom: boolean = true;
  public form: FormGroup = this.fb.group({});
  public tableDateFormat = TABLE_DATE_FORMAT;
  public isLoaded: boolean = false;
  public isNeedReloadData: boolean = true;
  public isFullListDisplayed: boolean = false;
  public chatMembers: Map<number, IUser> = new Map<number, IUser>();
  public params = new PaginationSorting();
  public selectedPageIndex: BehaviorSubject<number> = new BehaviorSubject<number>(-1);

  protected abstract messages: TChatMessage[];
  protected abstract chatBody: ElementRef;
  protected destroy$ = new Subject();

  private minLength: number = 2;
  private maxLength: number = 200;
  private noExistedMembersChanges = new Subject<number>();
  private noExistedMemberIds: number[] = [];

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

  public ngOnInit(): void {
    this.initForm();
    this.initSubscriptions();
  }

  public ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
    this.chatMembers.clear();
    this.isLoaded = false;
  }

  public get canView(): boolean {
    return this.authService.isLoggedIn() && this.entityId.getValue() > 0;
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
          this.scrollChatToLastMessage();
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

  public scrollChatToLastMessage(preventLoading: boolean = false): void {
    this.isFullListDisplayed = preventLoading;
    setTimeout(() => {
      if (this.chatBody?.nativeElement) {
        this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
      }
      this.isFullListDisplayed = false;
    }, 100);
  }

  public onElementsChanged(forceScroll: boolean = false): void {
    if (forceScroll || this.isNearBottom) {
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

  public isUserNearBottom(): boolean {
    if (!this.chatBody?.nativeElement) return false;
    const threshold = 150;
    const position = this.chatBody.nativeElement.scrollTop + this.chatBody.nativeElement.offsetHeight;
    const height = this.chatBody.nativeElement.scrollHeight;
    return position > height - threshold;
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
      .subscribe(() => this.scrollChatToLastMessage(true));
  }
}
