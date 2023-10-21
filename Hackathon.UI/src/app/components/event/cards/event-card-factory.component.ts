import {
  ChangeDetectorRef,
  Component, ComponentFactoryResolver,
  OnInit, Type,
  ViewChild
} from '@angular/core';
import { EventDirective } from "../event.directive";
import { EventStatus } from "../../../models/Event/EventStatus";
import { ActivatedRoute } from "@angular/router";
import { RouterService } from "../../../services/router.service";
import { SnackService } from "../../../services/snack.service";
import { Subject, takeUntil } from "rxjs";
import { Event } from "../../../models/Event/Event";
import { EventCreateEditCardComponent } from "./event-create-edit-card/event-create-edit-card.component";
import { finalize } from "rxjs/operators";
import { EventService } from "../../../services/event/event.service";
import { EventMainViewCardComponent } from "./event-main-view-card/event-main-view-card.component";
import { EventCardStartedComponent } from "./event-card-started/event-card-started.component";
import { EventCardPublishedComponent } from "./event-card-published/event-card-published.component";
import { EventCardFinishedComponent } from "./event-card-finished/event-card-finished.component";
import { EventErrorMessages } from "../../../common/error-messages/event-error-messages";
import { EventClient } from "../../../services/event/event.client";
import { ApprovalApplicationsService } from "../../../services/approval-applications/approval-applications.service";

@Component({
  selector: 'app-event-card-factory',
  template: `<ng-template event-item></ng-template>`,
})
export class EventCardFactoryComponent implements OnInit {

  @ViewChild(EventDirective, { static: true }) eventDirective: EventDirective;

  private isLoading: boolean = true;
  private event: Event = new Event();
  private eventId: number;
  private destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private eventHttpService: EventClient,
    private eventService: EventService,
    private router: RouterService,
    private activeRoute: ActivatedRoute,
    private componentFactoryResolver: ComponentFactoryResolver,
    private snackService: SnackService,
    private approvalApplicationsService: ApprovalApplicationsService,
    private cdr: ChangeDetectorRef,
  ) {
  }

  ngOnInit(): void {
    this.initEventId();
    this.initSubscription();
    this.loadData();
  }

  private loadData(): void {
    this.isLoading = true;
    this.eventHttpService.getById(this.eventId)
      .pipe(
        finalize(() => this.isLoading = false),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (event) => {
          if (event) {
            this.isLoading = false;
            this.event = event;
            this.renderEventCard();
          } else {
            this.goBack();
          }
        },
        error: () => this.goBack(),
      })
  }

  private renderEventCard(): void {
    const viewContainerRef = this.eventDirective?.viewContainerRef;
    if (!viewContainerRef) return;
    viewContainerRef.clear();
    const eventFactory = this.componentFactoryResolver
      .resolveComponentFactory<any>(this.getComponentByEventType());
    const eventRef = viewContainerRef.createComponent(eventFactory);
    eventRef.instance.event = this.event;
    eventRef.instance.isLoading = this.isLoading;
    this.cdr.detectChanges();
  }

  private getComponentByEventType():
    Type<
      EventCreateEditCardComponent |
      EventCardPublishedComponent |
      EventCardStartedComponent |
      EventCardFinishedComponent |
      EventMainViewCardComponent
      >
  {
    if (!Object.values(EventStatus).includes(Number(this.event?.status))) {
      this.goBack();
    }

    switch (this.event?.status) {
      case EventStatus.Draft:
      case EventStatus.OnModeration:
        return EventCreateEditCardComponent;
      case EventStatus.Published: return EventCardPublishedComponent;
      case EventStatus.Started: return EventCardStartedComponent;
      case EventStatus.Finished: return EventCardFinishedComponent;
      default: return EventMainViewCardComponent;
    }
  }

  private initEventId(): void {
    const { eventId } = this.activeRoute.snapshot.params;
    if (Number.isNaN(eventId)) {
      this.goBack();
    }
    this.eventId = eventId;
  }

  private initSubscription(): void {
    this.eventService.reloadEvent
      .pipe(takeUntil(this.destroy$))
      .subscribe((isReload: boolean) => {
        if (isReload) {
          this.loadData();
        }
      });
  }

  private goBack(): void {
    this.router.Events.List().then(_=>
      this.snackService.open(EventErrorMessages.EventNotFound));
  }
}
