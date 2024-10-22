import { Component } from '@angular/core';
import { EventCardBaseComponent } from '../components/event-card-base.component';
import { EventService } from '../../../../services/event/event.service';
import { Event } from '../../../../models/Event/Event';
import { EventStatusTranslator } from '../../../../models/Event/EventStatus';
import { AuthService } from '../../../../services/auth.service';
import { AppStateService } from '../../../../services/app-state.service';

@Component({
  selector: 'app-event-card-published',
  templateUrl: './event-card-published.component.html',
  styleUrls: ['./event-card-published.component.scss'],
})
export class EventCardPublishedComponent extends EventCardBaseComponent {
  public eventStatusTranslator = EventStatusTranslator;
  public userId: number;

  constructor(
    public eventService: EventService,
    private authService: AuthService,
    protected appStateService: AppStateService,
  ) {
    super(appStateService);
    this.userId = authService.getUserId() ?? 0;
  }

  public getUsersCount(): number {
    return Event.getUsersCount(this.event);
  }
}
