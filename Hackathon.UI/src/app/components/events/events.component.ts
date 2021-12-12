import {AfterViewInit, Component, ViewChild} from '@angular/core';
import {EventModel} from "../../models/EventModel";
import {EventsService} from "../../services/events.service";
import {BaseCollectionModel} from "../../models/BaseCollectionModel";
import {MatSnackBar} from "@angular/material/snack-bar";
import {TeamModel} from "../../models/Team/TeamModel";
import {Router} from "@angular/router";
import {PageEvent} from "@angular/material/paginator";
import {PageSettings, PageSettingsDefaults} from "../../models/PageSettings";
import {EventStatus} from "../../models/EventStatus";

@Component({
  selector: 'events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.scss']
})
export class EventsComponent implements AfterViewInit {

  events: EventModel[] = [];
  displayedColumns: string[] = ['id', 'name', 'start', 'status'];
  pageSettings: PageEvent = new PageEvent();

  constructor(private eventsService: EventsService, private router: Router, private snackBar: MatSnackBar) {

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
    this.router.navigate(['/event/'+team.id]);
  }

  setPageSettings(event:PageEvent){
    this.pageSettings = event;
    this.fetch();
  }

  getEventStatus(status:EventStatus){
    return EventStatus[status];
  }
}
