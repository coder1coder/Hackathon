import {Component, Input, OnInit} from '@angular/core';
import {Event} from "../../../../../models/Event/Event";
import {EventStatus} from "../../../../../models/Event/EventStatus";
import {SnackService} from "../../../../../services/snack.service";
import {RouterService} from "../../../../../services/router.service";
import {IProblemDetails} from "../../../../../models/IProblemDetails";
import {EventService} from "../../../../../services/event/event.service";
import {EventHttpService} from "../../../../../services/event/event.http-service";

@Component({
  selector: 'app-actions',
  templateUrl: './event-button-actions.component.html',
  styleUrls: ['./event-button-actions.component.scss']
})
export class EventButtonActionsComponent implements OnInit {
  @Input() event: Event;

  constructor(
    public eventService: EventService,
    private eventHttpService: EventHttpService,
    private snack: SnackService,
    private router: RouterService
  ) { }

  ngOnInit(): void {
  }

  @Input() submit: () => void;

  public createNewTeam(): void {
    if (this.event?.status !== EventStatus.Published) {
      this.snack.open('Событие должно быть опубликовано')
      return;
    }

    this.router.Teams.New(this.event.id);
  }

  public startEvent(): void {
    this.eventHttpService.setStatus(this.event.id, EventStatus.Started)
      .subscribe({
        next: (_) =>  {
          this.snack.open(`Событие начато`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  public enterToEvent(): void {
    this.eventHttpService.join(this.event.id)
      .subscribe({
        next: (_) => {
          this.snack.open(`Вы зарегистрировались на мероприятие`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  public leaveFromEvent(): void {
    this.eventHttpService.leave(this.event.id)
      .subscribe({
        next: (_) => {
          this.snack.open(`Вы покинули мероприятие`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  public finishEvent(): void {
    this.eventHttpService.setStatus(this.event.id, EventStatus.Finished)
      .subscribe({
        next: (_) =>  {
          this.snack.open(`Событие завершено`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  public deleteEvent(): void {
    this.eventHttpService.remove(this.event.id)
      .subscribe({
        next: (_) => {
          this.router.Events.List().then(_=>
            this.snack.open(`Событие удалено`));
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  public setPublish(): void {
    this.eventHttpService.setStatus(this.event.id, EventStatus.Published)
      .subscribe({
        next: () => {
          this.snack.open(`Событие опубликовано`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail)}
      });
  }

}
