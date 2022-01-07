import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute} from "@angular/router";
import {EventModel} from "../../../models/EventModel";
import {EventService} from "../../../services/event.service";

@Component({
  selector: 'team',
  templateUrl: './events.view.component.html',
  styleUrls: ['./events.view.component.scss']
})
export class EventsViewComponent implements AfterViewInit {

  eventId: number;
  event: EventModel | undefined;

  constructor(
    private activateRoute: ActivatedRoute,
    private eventsService: EventService,
    private snackBar: MatSnackBar) {

    this.eventId = activateRoute.snapshot.params['eventId'];

    this.eventsService.getById(this.eventId)
      .subscribe({
        next: (r: EventModel) =>  {
          this.event = r;
        },
        error: () => {}
      });

    this.eventsService = eventsService;
    this.snackBar = snackBar;
  }

  ngAfterViewInit(): void {
  }
}
