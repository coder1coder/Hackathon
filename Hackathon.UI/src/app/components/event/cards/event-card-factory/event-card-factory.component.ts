import {
  ChangeDetectorRef,
  Component, ComponentFactoryResolver,
  OnInit, Type,
  ViewChild
} from '@angular/core';
import {EventDirective} from "../../event.directive";
import {EventStatus} from "../../../../models/Event/EventStatus";
import {ActivatedRoute} from "@angular/router";
import {RouterService} from "../../../../services/router.service";
import {SnackService} from "../../../../services/snack.service";
import {EventHttpService} from "../../../../services/event/event.http-service";
import {Subject, takeUntil} from "rxjs";
import {Event} from "../../../../models/Event/Event";
import {EventCreateEditCardComponent} from "../event-create-edit-card/event-create-edit-card.component";
import {finalize} from "rxjs/operators";
import {EventService} from "../../../../services/event/event.service";
import {EventMainViewCardComponent} from "../event-main-view-card/event-main-view-card.component";
import {EventFinishedViewCardComponent} from "../event-finished-view-card/event-finished-view-card.component";

@Component({
  selector: 'app-event-card-factory',
  templateUrl: './event-card-factory.component.html',
  styleUrls: ['./event-card-factory.component.scss'],
})
export class EventCardFactoryComponent implements OnInit {

  @ViewChild(EventDirective, { static: true }) eventDirective: EventDirective;

  private isLoading: boolean = true;
  private event: Event = new Event();
  private eventId: number;
  private destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private eventHttpService: EventHttpService,
    private eventService: EventService,
    private router: RouterService,
    private activeRoute: ActivatedRoute,
    private componentFactoryResolver: ComponentFactoryResolver,
    private snackService: SnackService,
    private cdr: ChangeDetectorRef
  ) {
    this.initEventId();
  }

  ngOnInit(): void {
    this.initSubscription();
    this.loadData();
  }

  private loadData(): void {
    this.isLoading = true;
    this.eventHttpService.getById(this.eventId)
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => this.isLoading = false))
      .subscribe({
        next: (res: Event) => {
          if (res) {
            this.isLoading = false;
            this.event = res;
            this.renderEventCard();
          } else {
            this.goBack();
          }
        },
        error: () => this.goBack()
      })
  }

  private renderEventCard(): void {
    let viewContainerRef = this.eventDirective?.viewContainerRef;
    if (!viewContainerRef) return;
    viewContainerRef.clear();
    let eventFactory = this.componentFactoryResolver
      .resolveComponentFactory<any>(this.getComponentByEventStatus());
    let eventRef = viewContainerRef.createComponent(eventFactory);
    eventRef.instance.event = this.event;
    eventRef.instance.isLoading = this.isLoading;
    this.cdr.detectChanges();
  }

  private getComponentByEventStatus(): Type<any> {
    if (!Object.values(EventStatus).includes(Number(this.event?.status))) {
      this.goBack();
    }

    switch (this.event?.status) {
      case EventStatus.Draft: return EventCreateEditCardComponent;
      case EventStatus.Published: return EventMainViewCardComponent;
      case EventStatus.Started: return EventMainViewCardComponent;
      case EventStatus.Development: return EventMainViewCardComponent;
      case EventStatus.Prepare: return EventMainViewCardComponent;
      case EventStatus.Presentation: return EventMainViewCardComponent;
      case EventStatus.Decision: return EventMainViewCardComponent;
      case EventStatus.Award: return EventMainViewCardComponent;
      case EventStatus.Finished: return EventFinishedViewCardComponent;
    }
  }

  private initEventId(): void {
    const {eventId} = this.activeRoute.snapshot.params;
    if (Number(eventId)) {
      this.eventId = eventId;
    } else {
      this.goBack();
    }
  }

  private initSubscription(): void {
    this.eventService.reloadEvent.subscribe((isReload: boolean) => {
      if (isReload) {
        this.loadData();
      }
    });
  }

  private goBack(): void {
    this.router.Events.List().then(_=>
      this.snackService.open(`Событие не найдено`))
  }
}