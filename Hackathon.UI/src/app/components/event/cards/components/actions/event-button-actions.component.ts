import {Component, Input, OnInit} from '@angular/core';
import {Event} from "../../../../../models/Event/Event";
import {EventStatus} from "../../../../../models/Event/EventStatus";
import {SnackService} from "../../../../../services/snack.service";
import {RouterService} from "../../../../../services/router.service";
import {IProblemDetails} from "../../../../../models/IProblemDetails";
import {EventService} from "../../../../../services/event/event.service";
import {EventClient} from "../../../../../services/event/event.client";
import {CustomDialog, ICustomDialogData} from "../../../../custom/custom-dialog/custom-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {ErrorProcessor} from "../../../../../services/errorProcessor";

@Component({
  selector: 'event-button-actions',
  templateUrl: './event-button-actions.component.html',
  styleUrls: ['./event-button-actions.component.scss']
})
export class EventButtonActionsComponent implements OnInit {
  @Input() event: Event;

  constructor(
    public eventService: EventService,
    private eventHttpService: EventClient,
    private snack: SnackService,
    private router: RouterService,
    public dialog: MatDialog,
    private errorProcessor: ErrorProcessor,
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
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          const msg = (problemDetails?.detail || problemDetails["validation-error"]) ?? 'Неизвестная ошибка';
          this.snack.open(msg);
        }
      });
  }

  public enterToEvent(): void {

    if (this.event?.agreement?.requiresConfirmation == true)
    {
      const data: ICustomDialogData = {
        header: 'Правила участия в мероприятии',
        content: this.event.agreement.rules,
        acceptButtonText: `Принять`
      };

      this.dialog.open(CustomDialog, { data })
        .afterClosed()
        .subscribe(x => {
          if (x) {
            this.eventHttpService.acceptAgreement(this.event.id)
              .subscribe({
                next: (_) => {
                  this.joinToEvent();
                },
                error: errorContext => {
                  this.errorProcessor.Process(errorContext)
                }
              });
          }
        });
    } else this.joinToEvent();
  }

  private joinToEvent(){
    this.eventHttpService.join(this.event.id)
      .subscribe({
        next: (_) => {
          this.snack.open(`Вы зарегистрировались на мероприятии`);
          this.eventService.reloadEvent.next(true);
        },
        error: errorContext => {
          this.errorProcessor.Process(errorContext)
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
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          const msg = (problemDetails?.detail || problemDetails["validation-error"]) ?? 'Неизвестная ошибка';
          this.snack.open(msg);
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
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          const msg = (problemDetails?.detail || problemDetails["validation-error"]) ?? 'Неизвестная ошибка';
          this.snack.open(msg);
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
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          const msg = (problemDetails?.detail || problemDetails["validation-error"]) ?? 'Неизвестная ошибка';
          this.snack.open(msg);
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
          const problemDetails: IProblemDetails = <IProblemDetails>err.error;
          const msg = (problemDetails?.detail || problemDetails["validation-error"]) ?? 'Неизвестная ошибка';
          this.snack.open(msg);
        }
      });
  }

}
