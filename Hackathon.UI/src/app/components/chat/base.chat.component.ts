import {AuthService} from "../../services/auth.service";
import {AfterViewInit, ElementRef, Injectable, OnInit, ViewChild} from "@angular/core";
import {FormControl, FormGroup, NgForm, Validators} from "@angular/forms";
import {BehaviorSubject} from "rxjs";

@Injectable()
export abstract class BaseChatComponent<TChatMessage> implements OnInit, AfterViewInit {

  currentUserId:number;
  isOpened = false
  isFloatMode = false;

  @ViewChild('formComponent') formComponent: NgForm;

  protected abstract messages:TChatMessage[];
  form:FormGroup;

  protected constructor(
    protected authService: AuthService) {
    this.currentUserId = this.authService.getUserId() ?? -1;
    authService.authChange.subscribe(_ => {
      this.fetchEntity();
      this.fetchMessages();
    })
  }

  ngAfterViewInit(): void {
    this.fetchMessages();
  }

  ngOnInit() {

    this.initForm();

    this.entityId.subscribe(value=>{

      if (value < 1)
        return;

      this.fetchEntity();
      this.fetchMessages();
    })
  }

  initForm():void{
    this.form = new FormGroup({
      message: new FormControl('',[
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(200)
      ]),
      notify: new FormControl(false, [
        Validators.required
      ]),
    })
  }

  public get canView():boolean{
    return this.authService.isLoggedIn() && this.entityId.getValue() > 0;
  }

  abstract chatBody: ElementRef | undefined;

  abstract get canSendMessageWithNotify():boolean;

  abstract entityId: BehaviorSubject<number>;
  abstract fetchEntity():void;
  abstract fetchMessages():void;
  abstract sendMessage():void;

  protected scrollChatToLastMessage(){
    if (this.chatBody !== undefined) {
      let bottomPosition = Math.max(0, this.chatBody.nativeElement.scrollHeight - this.chatBody.nativeElement.offsetHeight);
      this.chatBody.nativeElement.scrollTop = bottomPosition;
    }
  }

}
