import {Component, OnInit} from '@angular/core';
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {Event} from "../../../../models/Event/Event";
import {DATE_FORMAT} from "../../../../common/date-formats";
import * as moment from "moment/moment";
import {RouterService} from "../../../../services/router.service";
import {Team} from "../../../../models/Team/Team";
import {AuthService} from "../../../../services/auth.service";

@Component({
  selector: 'app-event-finished-view-card',
  templateUrl: './event-finished-view-card.component.html',
  styleUrls: ['./event-finished-view-card.component.scss']
})
export class EventFinishedViewCardComponent extends EventCardBaseComponent implements OnInit {

  private readonly userId: number;

  constructor(
    public router: RouterService,
    private authService: AuthService
  ) {
    super();
    this.userId = authService.getUserId() ?? 0;
  }

  ngOnInit(): void {
  }

  public get membersCount(): number {
    return Event.getUsersCount(this.event);
  }

  public get eventTeamsCount(): number {
    return this.event?.teams?.length ?? 0;
  }

  public get maxEventMembers(): number {
    return this.event?.maxEventMembers
  }

  public get startDate(): string {
    return moment(this.event?.start).local().format(DATE_FORMAT)
  }

  public goToTeamCard = (item: Team) => {
    if (item.owner?.id === this.userId) {
      this.router.Teams.MyTeam();
    } else {
      this.router.Teams.View(item.id);
    }
  };
}
