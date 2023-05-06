import { Component, OnInit } from '@angular/core';
import { EventCardBaseComponent } from "../components/event-card-base.component";
import { EventService } from "../../../../services/event/event.service";
import { Event } from "../../../../models/Event/Event";
import { EventStatusTranslator } from "../../../../models/Event/EventStatus";
import { AuthService } from "../../../../services/auth.service";

@Component({
  selector: 'app-event-card-published',
  templateUrl: './event-card-published.component.html',
  styleUrls: ['./event-card-published.component.scss']
})
export class EventCardPublishedComponent extends EventCardBaseComponent implements OnInit {

  public eventStatusTranslator = EventStatusTranslator;
  public userId: number;

  constructor(
    public eventService: EventService,
    private authService: AuthService
  ) {
    super();
    this.userId = authService.getUserId() ?? 0;
  }

  ngOnInit(): void {}

  public getUsersCount(event: Event): number {
    return Event.getUsersCount(this.event);
  }
}