import { Component, Input } from '@angular/core';
import { EventStatus, EventStatusTranslator } from '../../../../../models/Event/EventStatus';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'app-event-status',
    templateUrl: './event-status.component.html',
    styleUrls: ['./event-status.component.scss'],
    imports: [MatIcon]
})
export class EventStatusComponent {
  @Input() status!: EventStatus;
  public eventStatusTranslator = EventStatusTranslator;
}
