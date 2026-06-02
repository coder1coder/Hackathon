import { Component, Input } from '@angular/core';
import { EventStatus, EventStatusTranslator } from '../../../../../models/Event/EventStatus';

@Component({
    selector: 'app-event-status',
    templateUrl: './event-status.component.html',
    styleUrls: ['./event-status.component.scss'],
    standalone: false
})
export class EventStatusComponent {
  @Input() status: EventStatus;
  public eventStatusTranslator = EventStatusTranslator;
}
