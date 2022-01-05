import {AfterViewInit, Component} from '@angular/core';
import {EventModel} from "../../models/EventModel";
import {EventService} from "../../services/event.service";
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'team',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss']
})
export class EventComponent implements AfterViewInit {

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
