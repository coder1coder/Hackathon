import {Component, Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";
import {IProject} from "../../../../models/Project/IProject";
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {AuthService} from "../../../../services/auth.service";
import {Event} from "../../../../models/Event/Event";

@Component({
  selector: `event-card-started`,
  styleUrls: [`event-card-started.component.scss`],
  templateUrl: `event-card-started.component.html`
})

@Injectable()
export class EventCardStartedComponent extends EventCardBaseComponent
{
  Pages = Pages;
  Event = Event;

  set selectedChatIndex(value) { this._selectedChatIndex.next(value); };
  get selectedChatIndex() { return this._selectedChatIndex.getValue(); }
  private _selectedChatIndex = new BehaviorSubject<number>(0);

  currentUserId: number | undefined;
  selectedPageIndex: number = Pages.Communication;
  project: IProject | undefined;
  sideBarWidthPx: number = 200;
  currentChatId: number = -1;

  constructor(private authService:AuthService) {
    super();

    this.currentUserId = this.authService.getUserId();

    this._selectedChatIndex.subscribe((value:number) =>{
      switch (value){
        case Chat.EventChat:

          this.currentChatId = -1;

          break;

        case Chat.TeamChat:

          let team = this.event.teams.find(x=> x.members.find(m=>m.id == this.currentUserId));

          if (team)
            this.currentChatId = team.id;

          break;
      }
    })
  }
}

enum Pages {
  EventTasks = 0,
  Communication = 1,
  Project = 2
}

enum Chat {
  EventChat = 0,
  TeamChat = 1
}
