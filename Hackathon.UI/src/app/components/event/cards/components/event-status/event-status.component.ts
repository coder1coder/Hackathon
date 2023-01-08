import {Component, Input, OnInit} from '@angular/core';
import {EventStatus, EventStatusTranslator} from "../../../../../models/Event/EventStatus";

@Component({
  selector: 'app-event-status',
  templateUrl: './event-status.component.html',
  styleUrls: ['./event-status.component.scss']
})
export class EventStatusComponent implements OnInit {

  @Input() status: EventStatus;
  public eventStatusTranslator = EventStatusTranslator;

  constructor() { }

  ngOnInit(): void {
  }

}
