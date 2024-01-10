import { Component, Input, OnDestroy } from '@angular/core';
import { Event } from '../../../../../models/Event/Event';
import { EventStatus } from '../../../../../models/Event/EventStatus';
import { SnackService } from '../../../../../services/snack.service';
import { RouterService } from '../../../../../services/router.service';
import { EventService } from '../../../../../services/event/event.service';
import { EventClient } from '../../../../../services/event/event.client';
import {
  CustomDialogComponent,
  ICustomDialogData,
} from '../../../../custom/custom-dialog/custom-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { ErrorProcessorService } from '../../../../../services/error-processor.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'event-button-actions',
  templateUrl: './event-button-actions.component.html',
  styleUrls: ['./event-button-actions.component.scss'],
})
export class EventButtonActionsComponent implements OnDestroy {
  @Input() event: Event;
  @Input() submit: () => void;
  @Input() formValidity: boolean = true;

  private destroy$ = new Subject();

  constructor(
    public eventService: EventService,
    private eventHttpService: EventClient,
    private snack: SnackService,
    private router: RouterService,
    public dialog: MatDialog,
    private errorProcessor: ErrorProcessorService,
  ) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public createNewTeam(): void {
    if (this.event?.status !== EventStatus.Published) {
      this.snack.open('Событие должно быть опубликовано');
      return;
    }

    this.router.Teams.New(this.event.id);
  }

  public startEvent(): void {
    this.eventHttpService
      .setStatus(this.event.id, EventStatus.Started)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Событие начато`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }

  public enterToEvent(): void {
    if (this.event?.agreement?.requiresConfirmation) {
      const data: ICustomDialogData = {
        header: 'Правила участия в мероприятии',
        content: this.event.agreement.rules,
        acceptButtonText: `Принять`,
      };

      this.dialog
        .open(CustomDialogComponent, { data })
        .afterClosed()
        .pipe(takeUntil(this.destroy$))
        .subscribe((res) => {
          if (res) {
            this.eventHttpService
              .acceptAgreement(this.event.id)
              .pipe(takeUntil(this.destroy$))
              .subscribe({
                next: () => {
                  this.joinToEvent();
                },
                error: (err) => this.errorProcessor.Process(err),
              });
          }
        });
    } else this.joinToEvent();
  }

  private joinToEvent(): void {
    this.eventHttpService
      .join(this.event.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Вы зарегистрировались на мероприятии`);
          this.eventService.reloadEvent.next(true);
        },
        error: (errorContext) => {
          this.errorProcessor.Process(errorContext);
        },
      });
  }

  public leaveFromEvent(): void {
    this.eventHttpService
      .leave(this.event.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Вы покинули мероприятие`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }

  public finishEvent(): void {
    this.eventHttpService
      .setStatus(this.event.id, EventStatus.Finished)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Событие завершено`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }

  public deleteEvent(): void {
    this.eventHttpService
      .remove(this.event.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.router.Events.List().then(() => this.snack.open(`Событие удалено`));
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }

  public setPublish(): void {
    this.eventHttpService
      .setStatus(this.event.id, EventStatus.Published)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Событие опубликовано`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }

  public setOnModeration(): void {
    this.eventHttpService
      .setStatus(this.event.id, EventStatus.OnModeration)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.snack.open(`Событие отправлено на модерацию`);
          this.eventService.reloadEvent.next(true);
        },
        error: (err) => this.errorProcessor.Process(err),
      });
  }
}
