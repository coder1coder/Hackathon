import { AuthService } from "../../services/auth.service";
import { ElementRef, HostListener, Injectable, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, NgForm, Validators } from "@angular/forms";
import { BehaviorSubject, from, mergeMap, Subject, takeUntil } from "rxjs";
import { TABLE_DATE_FORMAT } from "../../common/consts/date-formats";
import { WithFormBaseComponent } from "../../common/base-components/with-form-base.component";
import { ProfileUserStore } from "../../shared/stores/profile-user.store";
import { IUser } from "../../models/User/IUser";

@Injectable()
export abstract class BaseChatComponent<TChatMessage>
  extends WithFormBaseComponent implements OnInit, OnDestroy {

  @ViewChild('formComponent') formComponent: NgForm;
  public currentUserId: number;
  public isOpened: boolean = false
  public isFloatMode: boolean = false;
  public form: FormGroup = this.fb.group({});
  public tableDateFormat = TABLE_DATE_FORMAT;
  public isLoaded: boolean = false;
  public chatMembers: Map<number, IUser> = new Map<number, IUser>();

  protected abstract messages: TChatMessage[];
  protected abstract chatBody: ElementRef;
  protected destroy$ = new Subject();

  private minLength: number = 2;
  private maxLength: number = 200;

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

  public get canView():boolean{
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
        complete: () => this.isLoaded = true,
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

  public sortMessages(): TChatMessage[] {
    this.scrollChatToLastMessage();
    return this.messages = this.messages.sort((a: TChatMessage, b: TChatMessage) => {
      const timestampA = (a as any).timestamp as Date;
      const timestampB = (b as any).timestamp as Date;
      return <any>new Date(timestampA) - <any>new Date(timestampB);
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
    if (this.chatBody !== undefined) {
      this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
    }
  }

  public getMember(id: number): IUser {
    return this.chatMembers.get(id);
  }

  private initSubscriptions(): void {
    this.entityId
      .pipe(takeUntil(this.destroy$))
      .subscribe((value: number) => {
        if (value < 1) return;
        this.fetchEntity(true);
        this.fetchMessages();
      });

    this.currentUserId = this.authService.getUserId() ?? -1;
    this.authService.authChange
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.fetchEntity(true);
        this.fetchMessages();
      });
  }
}
