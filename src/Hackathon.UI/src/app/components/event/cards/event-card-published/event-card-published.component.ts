import { Component, inject } from '@angular/core';
import { EventCardBaseComponent } from '../components/event-card-base.component';
import { EventService } from '../../../../services/event/event.service';
import { Event } from '../../../../models/Event/Event';
import { EventStatusTranslator } from '../../../../models/Event/EventStatus';
import { AuthService } from '../../../../services/auth.service';
import { AppStateService } from '../../../../services/app-state.service';
import { DefaultLayoutComponent } from '../../../layouts/default/default.layout.component';
import { AsyncPipe } from '@angular/common';
import { AlertComponent } from '../../../custom/alert/alert.component';
import { EventButtonActionsComponent } from '../components/actions/event-button-actions.component';
import { EventHeaderComponent } from '../components/event-header/event-header.component';
import { ImageFromStorageComponent } from '../../../custom/image-from-storage/image-from-storage.component';

@Component({
    selector: 'app-event-card-published',
    templateUrl: './event-card-published.component.html',
    styleUrls: ['./event-card-published.component.scss'],
    imports: [DefaultLayoutComponent, AlertComponent, EventButtonActionsComponent, EventHeaderComponent, ImageFromStorageComponent, AsyncPipe]
})
export class EventCardPublishedComponent extends EventCardBaseComponent {
  eventService = inject(EventService);
  private authService = inject(AuthService);
  protected appStateService = inject(AppStateService);

  public eventStatusTranslator = EventStatusTranslator;
  public userId: number;

  constructor() {
    super();
    this.userId = this.authService.getUserId() ?? 0;
  }

  public getUsersCount(): number {
    return Event.getUsersCount(this.event);
  }
}
