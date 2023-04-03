import {AuthService} from "../../services/auth.service";
import {AfterViewInit, ElementRef, Injectable, ViewChild} from "@angular/core";
import {NgForm} from "@angular/forms";

@Injectable()
export abstract class BaseChatComponent<TChatMessage> implements AfterViewInit {

  currentUserId:number;
  isOpened = false
  isFloatMode = false;

  @ViewChild('scrollMe') protected chatBody: ElementRef | undefined;
  @ViewChild('formComponent') formComponent: NgForm;

  public messages:TChatMessage[] = []

  protected constructor(
    protected authService: AuthService) {

    this.currentUserId = this.authService.getUserId() ?? -1;
    authService.authChange.subscribe(_ => this.updateChatView())
  }

  ngAfterViewInit(): void {
    this.updateChatView()
  }

  abstract get canView():boolean;
  abstract get canSendMessageWithNotify():boolean;

  abstract fetchMessages():void;
  abstract updateChatView():void;
  abstract sendMessage():void;

  protected scrollChatToLastMessage(){
    setTimeout(()=>{
      if (this.chatBody !== undefined) {
        this.chatBody.nativeElement.scrollTop = this.chatBody.nativeElement.scrollHeight;
      }
    },100)
  }

}
