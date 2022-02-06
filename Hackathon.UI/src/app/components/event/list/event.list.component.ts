import {Component} from '@angular/core';
import {Router} from "@angular/router";
import {PageSettings} from "../../../models/PageSettings";
import {EventService} from "../../../services/event.service";
import {EventModel} from "../../../models/EventModel";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {EventStatusTranslator, EventStatus} from "../../../models/EventStatus";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {TeamModel} from "../../../models/Team/TeamModel";

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss']
})

export class EventListComponent extends BaseTableListComponent<EventModel>{

  EventStatusTranslator = EventStatusTranslator;

  override getDisplayColumns(): string[] {
    return ['id', 'name', 'start', 'status', 'user', 'teams', 'members', 'actions'];
  }

  constructor(private eventsService: EventService, private router: Router) {
    super(EventListComponent.name);
  }

  fetch(){
    this.eventsService.getAll(new PageSettings(this.pageSettings))
      .subscribe({
        next: (r: BaseCollectionModel<EventModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  rowClick(event: EventModel){
    this.router.navigate(['/events/'+event.id]);
  }

  getEventStatus(status:EventStatus){
    return EventStatus[status].toLowerCase();
  }

  getUsersCount(event:EventModel){
    let i = 0;
    this.getEventTeams(event)?.forEach(x=> i += x.users?.length ?? 0);
    return i;
  }

  showCreateEventPage(){
    this.router.navigate(['/events/new']);
  }

  getEventTeams(event:EventModel):TeamModel[] | undefined{
    return event?.teamEvents?.map(x=>x.team);
  }
}
