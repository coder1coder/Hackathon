import { Component } from '@angular/core';
import { EventCardBaseComponent } from '../components/event-card-base.component';
import { EventService } from '../../../../services/event/event.service';
import { Event } from '../../../../models/Event/Event';
import { EventStatusTranslator } from '../../../../models/Event/EventStatus';
import { AuthService } from '../../../../services/auth.service';
import { AppStateService } from '../../../../services/app-state.service';
import { DefaultLayoutComponent } from '../../../layouts/default/default.layout.component';
import { NgIf, NgFor, AsyncPipe } from '@angular/common';
import { AlertComponent } from '../../../custom/alert/alert.component';
import { EventButtonActionsComponent } from '../components/actions/event-button-actions.component';
import { EventHeaderComponent } from '../components/event-header/event-header.component';
import { ImageFromStorageComponent } from '../../../custom/image-from-storage/image-from-storage.component';

@Component({
    selector: 'app-event-card-published',
    templateUrl: './event-card-published.component.html',
    styleUrls: ['./event-card-published.component.scss'],
    imports: [DefaultLayoutComponent, NgIf, AlertComponent, EventButtonActionsComponent, EventHeaderComponent, ImageFromStorageComponent, NgFor, AsyncPipe]
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
