import {Component, ViewChild} from '@angular/core';
import {EventService} from "../../../services/event.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {EventStatus, EventStatusTranslator} from "../../../models/Event/EventStatus";
import {BaseTableListComponent} from "../../BaseTableListComponent";
import {FormControl, FormGroup} from "@angular/forms";
import {EventFilter} from "../../../models/Event/EventFilter";
import {GetListParameters} from "../../../models/GetListParameters";
import {Event} from 'src/app/models/Event/Event';
import * as moment from "moment/moment";
import {RouterService} from "../../../services/router.service";
import {MatSelect} from "@angular/material/select";
import {IEventListItem} from "../../../models/Event/IEventListItem";
import {AuthService} from "../../../services/auth.service";

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss']
})

export class EventListComponent extends BaseTableListComponent<IEventListItem>{

  EventStatusTranslator = EventStatusTranslator;
  EventModel = Event;
  moment = moment

  filterForm = new FormGroup({
    name: new FormControl(),
    startFrom: new FormControl(),
    startTo: new FormControl(),
    statuses: new FormControl(),
    iAmOwner: new FormControl()
  })

  @ViewChild('statuses') statusesSelect!: MatSelect;

  override getDisplayColumns(): string[] {
    return ['name', 'start', 'status', 'user', 'teams', 'members', 'actions'];
  }

  constructor(
    public eventsService: EventService,
    public router: RouterService,
    private authService: AuthService
  ) {
    super(EventListComponent.name);
  }

  override fetch(){

    let params = new GetListParameters<EventFilter>();

    if (this.pageSettings != undefined)
    {
      params.Offset = this.pageSettings.pageIndex * this.pageSettings.pageSize;
      params.Limit = this.pageSettings.pageSize;
    }

    params.Filter = new EventFilter();
    params.Filter = this.filterForm.value;

    if (this.filterForm.value.iAmOwner)
    {
      let userId = this.authService.getUserId();

      if (userId)
        params.Filter.ownerIds = [userId]
    }

    params.Filter.excludeOtherUsersDraftedEvents = true;

      this.eventsService.getList(params)
      .subscribe({
        next: (r: BaseCollection<IEventListItem>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  statusesToggleAll(event:any){

    if (event.target.tag == 0 || event.target.tag == undefined)
    {
      let all = this.statusesSelect?.options.map(x=>x.value);
      this.filterForm.controls['statuses'].patchValue(all);
      event.target.tag = 1;
    } else {
      this.filterForm.controls['statuses'].patchValue([]);
      event.target.tag = 0;
    }
  }

  clearFilter(){
    this.filterForm.reset();
    this.fetch();
  }

  rowClick(event: IEventListItem){
    if (event.status == EventStatus.Draft)
      this.router.Events.Edit(event.id);
    else
      this.router.Events.View(event.id);
  }

  getEventStatus(status:EventStatus){
    return EventStatus[status].toLowerCase();
  }

  getAllEventStatuses():any[]{
    return Object
      .keys(EventStatus)
      .filter(k => !isNaN(Number(k)))
      .map(x => Number(x));
  }
}
