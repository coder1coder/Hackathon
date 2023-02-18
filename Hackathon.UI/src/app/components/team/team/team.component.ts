import {Component, Input} from "@angular/core";
import {Team} from "../../../models/Team/Team";
import {RouterService} from "../../../services/router.service";
import {EventHttpService} from "../../../services/event/event.http-service";
import {EventFilter} from "../../../models/Event/EventFilter";
import {GetListParameters} from "../../../models/GetListParameters";
import {MatTabChangeEvent} from "@angular/material/tabs";
import {IEventListItem} from "../../../models/Event/IEventListItem";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})

export class TeamComponent {

  @Input() team: Team;

  public teamEvents: IEventListItem[] = [];
  public userId = this.authService.getUserId();

  constructor(
    public router: RouterService,
    private eventHttpService: EventHttpService,
    private authService: AuthService
  ) {
  }

  public get isTeamMember(): boolean {
    if (!Boolean(this.team)) return false;
    if (this.team.owner?.id === this.userId) return true;
    return this.team?.members.some(member => member.id === this.userId);
  }

  public tabChanged(event: MatTabChangeEvent): void {
    if (event.index == 2 && this.team != null) {
      let getList = new GetListParameters<EventFilter>();
      getList.Filter = new EventFilter();
      getList.Filter.teamsIds = [ this.team.id ];

      this.eventHttpService.getList(getList)
        .subscribe(x => this.teamEvents = x.items);
    }
  }
}
