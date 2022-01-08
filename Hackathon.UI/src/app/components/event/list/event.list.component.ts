import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
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
  displayedColumns: string[] = ['id', 'name', 'start', 'status', 'user', 'teams', 'members'];
  pageSettings: PageEvent = new PageEvent();

  constructor(private eventsService: EventService, private router: Router, private snackBar: MatSnackBar) {

    this.pageSettings.pageSize = PageSettingsDefaults.PageSize;
    this.pageSettings.pageIndex = PageSettingsDefaults.PageIndex;
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

  handleRowClick(team: TeamModel){
    this.router.navigate(['/events/'+team.id]);
  }

  setPageSettings(event:PageEvent){
    this.pageSettings = event;
    this.fetch();
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
