import {Component, Input} from "@angular/core";
import {Team} from "../../../models/Team/Team";
import {RouterService} from "../../../services/router.service";
import {EventService} from "../../../services/event.service";
import {EventFilter} from "../../../models/Event/EventFilter";
import {GetListParameters} from "../../../models/GetListParameters";
import {MatTabChangeEvent} from "@angular/material/tabs";
import {IEventListItem} from "../../../models/Event/IEventListItem";

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent {

  @Input() team?: Team

  public teamEvents:IEventListItem[] = [];

  constructor(
    public router:RouterService,
    private eventService:EventService) {
  }

  public tabChanged(event: MatTabChangeEvent){

    if (event.index == 2 && this.team != null)
    {
      let getList = new GetListParameters<EventFilter>();
      getList.Filter = new EventFilter();
      getList.Filter.teamsIds = [ this.team.id ];


      this.eventService.getList(getList)
        .subscribe(x => this.teamEvents = x.items);
    }
  }
}
