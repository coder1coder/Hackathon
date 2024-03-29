import {
  ChangeDetectorRef,
  Component,
  ComponentFactory,
  ComponentFactoryResolver,
  ComponentRef,
  OnDestroy,
  OnInit,
  Type,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { EventDirective } from '../event.directive';
import { EventStatus } from '../../../models/Event/EventStatus';
import { ActivatedRoute } from '@angular/router';
import { RouterService } from '../../../services/router.service';
import { SnackService } from '../../../services/snack.service';
import { forkJoin, of, Subject, switchMap, takeUntil } from 'rxjs';
import { Event } from '../../../models/Event/Event';
import { EventCreateEditCardComponent } from './event-create-edit-card/event-create-edit-card.component';
import { finalize } from 'rxjs/operators';
import { EventService } from '../../../services/event/event.service';
import { EventMainViewCardComponent } from './event-main-view-card/event-main-view-card.component';
import { EventCardStartedComponent } from './event-card-started/event-card-started.component';
import { EventCardPublishedComponent } from './event-card-published/event-card-published.component';
import { EventCardFinishedComponent } from './event-card-finished/event-card-finished.component';
import { EventErrorMessages } from '../../../common/error-messages/event-error-messages';
import { AuthService } from '../../../services/auth.service';
import { GlobalErrorHandler } from '../../../common/handlers/error.handler';
import { AppStateService } from '../../../services/app-state.service';
import { EventsClient } from 'src/app/clients/events.client';

@Component({
  selector: 'app-event-card-factory',
  template: `<ng-template event-item></ng-template>`,
})
export class EventCardFactoryComponent implements OnInit, OnDestroy {
  @ViewChild(EventDirective, { static: true }) eventDirective: EventDirective;

  private event: Event = new Event();
  private eventId: number;
  private destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private authService: AuthService,
    private eventsClient: EventsClient,
    private eventService: EventService,
    private routerService: RouterService,
    private activeRoute: ActivatedRoute,
    private componentFactoryResolver: ComponentFactoryResolver,
    private snackService: SnackService,
    private globalErrorHandler: GlobalErrorHandler,
    private cdr: ChangeDetectorRef,
    private appStateService: AppStateService,
  ) {}

  ngOnInit(): void {
    this.initEventId();
    this.initSubscription();
    this.loadData();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private loadData(): void {
    this.appStateService.setIsLoadingState(true);
    this.eventsClient
      .getById(this.eventId)
      .pipe(
        switchMap((event: Event) =>
          forkJoin([this.eventService.checkAccessViewEventByModel(event), of(event)]),
        ),
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: ([isCanView, event]) => {
          this.appStateService.setIsLoadingState(false);
          if (isCanView) {
            this.event = event;
            this.renderEventCard();
          } else {
            this.routerService.Events.List().then(() =>
              this.snackService.open(EventErrorMessages.EventNoAccess),
            );
          }
        },
        error: (err) => this.globalErrorHandler.handleError(err),
      });
  }

  private renderEventCard(): void {
    const viewContainerRef: ViewContainerRef = this.eventDirective?.viewContainerRef;
    if (!viewContainerRef) return;
    viewContainerRef.clear();
    const eventFactory: ComponentFactory<any> =
      this.componentFactoryResolver.resolveComponentFactory<any>(this.getComponentByEventType());
    const eventRef: ComponentRef<any> = viewContainerRef.createComponent(eventFactory);
    eventRef.instance.event = this.event;
    this.cdr.detectChanges();
  }

  private getComponentByEventType(): Type<
    | EventCreateEditCardComponent
    | EventCardPublishedComponent
    | EventCardStartedComponent
    | EventCardFinishedComponent
    | EventMainViewCardComponent
  > {
    if (!Object.values(EventStatus).includes(Number(this.event?.status))) {
      this.goBack();
    }

    switch (this.event?.status) {
      case EventStatus.Draft:
      case EventStatus.OnModeration:
        return EventCreateEditCardComponent;
      case EventStatus.Published:
        return EventCardPublishedComponent;
      case EventStatus.Started:
        return EventCardStartedComponent;
      case EventStatus.Finished:
        return EventCardFinishedComponent;
      default:
        return EventMainViewCardComponent;
    }
  }

  private initEventId(): void {
    if (!this.authService.isLoggedIn()) {
      this.routerService.Profile.Login();
    }

    const { eventId } = this.activeRoute.snapshot.params;
    if (Number.isNaN(eventId)) {
      this.goBack();
    }
    this.eventId = eventId;
  }

  private initSubscription(): void {
    this.eventService.reloadEvent.pipe(takeUntil(this.destroy$)).subscribe((isReload: boolean) => {
      if (isReload) {
        this.loadData();
      }
    });
  }

  private goBack(): void {
    this.routerService.Events.List().then(() =>
      this.snackService.open(EventErrorMessages.EventNotFound),
    );
  }
}
