import { Component, Input } from '@angular/core';
import * as moment from "moment/moment";
import { DATE_FORMAT_DD_MM_YYYY } from "../../../../../common/consts/date-formats";
import { Event } from "../../../../../models/Event/Event";

@Component({
  selector: 'app-event-header',
  templateUrl: './event-header.component.html',
  styleUrls: ['./event-header.component.scss'],
})
export class EventHeaderComponent {
  @Input() event: Event;

  public get startDate(): string {
    return moment(this.event?.start).local().format(DATE_FORMAT_DD_MM_YYYY);
  }
}
