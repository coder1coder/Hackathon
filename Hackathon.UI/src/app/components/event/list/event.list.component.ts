import {AfterViewInit, Component} from '@angular/core';
import {Router} from "@angular/router";
import {PageEvent} from "@angular/material/paginator";
import {PageSettings, PageSettingsDefaults} from "../../../models/PageSettings";
import {EventService} from "../../../services/event.service";
import {EventModel} from "../../../models/EventModel";
import {BaseCollectionModel} from "../../../models/BaseCollectionModel";
import {TeamModel} from "../../../models/Team/TeamModel";
import {EventStatus} from "../../../models/EventStatus";

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss']
})
export class EventListComponent implements AfterViewInit {

  events: EventModel[] = [];
  displayedColumns: string[] = ['id', 'name', 'start', 'status', 'user', 'teams', 'members', 'actions'];
  pageSettings: PageEvent = new PageEvent();

  constructor(private eventsService: EventService, private router: Router) {

    let pageSettingsJson = sessionStorage.getItem(`${EventListComponent.name}${PageEvent.name}`);

    if (pageSettingsJson != null)
      this.pageSettings = JSON.parse(pageSettingsJson)
    else
    {
      this.pageSettings.pageSize = PageSettingsDefaults.PageSize;
      this.pageSettings.pageIndex = PageSettingsDefaults.PageIndex;
    }
  }

  setPageSettings(event:PageEvent){
    this.pageSettings = event;
    sessionStorage.setItem(`${EventListComponent.name}${PageEvent.name}`, JSON.stringify(event));

    this.fetch();
  }

  ngAfterViewInit(): void {
    this.fetch();
  }

  fetch(){
    this.eventsService.getAll(new PageSettings(this.pageSettings))
      .subscribe({
        next: (r: BaseCollectionModel<EventModel>) =>  {
          this.events = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  handleRowClick(event: EventModel){
    this.router.navigate(['/events/'+event.id]);
  }

  getEventStatus(status:EventStatus){
    return EventStatus[status].toLowerCase();
  }

  getUsersCount(event:EventModel){
    let i = 0;
    event.teams.forEach(x=> i += x.users?.length ?? 0);
    return i;
  }

  showCreateEventPage(){
    this.router.navigate(['/events/new']);
  }
}
