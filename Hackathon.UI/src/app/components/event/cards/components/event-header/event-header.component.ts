import { Component, Input, OnInit } from '@angular/core';
import * as moment from "moment/moment";
import { DATE_FORMAT } from "../../../../../common/date-formats";
import { Event } from "../../../../../models/Event/Event";

@Component({
  selector: 'app-event-header',
  templateUrl: './event-header.component.html',
  styleUrls: ['./event-header.component.scss']
})
export class EventHeaderComponent implements OnInit {

  @Input() event: Event;

  constructor() { }

  ngOnInit(): void {
  }

  public get startDate(): string {
    return moment(this.event?.start).local().format(DATE_FORMAT)
  }
}
