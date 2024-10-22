import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseCollection } from '../../../models/BaseCollection';
import { EventStatus, EventStatusTranslator } from '../../../models/Event/EventStatus';
import { FormBuilder } from '@angular/forms';
import { EventFilter } from '../../../models/Event/EventFilter';
import { GetListParameters } from '../../../models/GetListParameters';
import * as moment from 'moment/moment';
import { RouterService } from '../../../services/router.service';
import { MatSelect } from '@angular/material/select';
import { IEventListItem } from '../../../models/Event/IEventListItem';
import { AuthService } from '../../../services/auth.service';
import { PageSettingsDefaults } from '../../../models/PageSettings';
import { DATE_FORMAT_DD_MM_YYYY } from '../../../common/consts/date-formats';
import { Observable, Subject, takeUntil } from 'rxjs';
import { EventService } from '../../../services/event/event.service';
import { EventErrorMessages } from '../../../common/error-messages/event-error-messages';
import { SnackService } from '../../../services/snack.service';
import { fromMobx } from '../../../common/functions/from-mobx.function';
import { AppStateService } from '../../../services/app-state.service';
import { finalize } from 'rxjs/operators';
import { EventsClient } from 'src/app/clients/events.client';

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss'],
})
export class EventListComponent implements OnInit {
  public filterForm = this.fb.group({});
  public eventList: BaseCollection<IEventListItem> = {
    items: [],
    totalCount: 0,
  };
  public eventStatusTranslator = EventStatusTranslator;
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);
  public isFullListDisplayed: boolean = false;
  public isFilterEnabled: boolean = false;

  private params = new GetListParameters<EventFilter>();
  private destroy$: Subject<boolean> = new Subject<boolean>();

  @ViewChild('statuses') statusesSelect: MatSelect;
  constructor(
    public router: RouterService,
    private eventsClient: EventsClient,
    private eventService: EventService,
    private snackService: SnackService,
    private authService: AuthService,
    private fb: FormBuilder,
    private appStateService: AppStateService,
  ) {}

  ngOnInit(): void {
    this.initDefaultSettings();
    this.initFormFilter();
    this.fetch(true);
  }

  public get layoutCssClasses(): string {
    return `container-event hackathon-background ${this.eventList.items.length < 6 ? 'h-100' : ''}`;
  }

  public getDateTimeFormat(date: Date): string {
    return moment(date).local().format(DATE_FORMAT_DD_MM_YYYY);
  }

  public fetch(reloadLimit?: boolean): void {
    this.fillParamsFromFilter(reloadLimit);
    this.loadData(this.params);
  }

  public statusesToggleAll(event: any): void {
    if (event.target.tag == 0 || event.target.tag == undefined) {
      const all: string[] | number[] = this.statusesSelect?.options.map((x) => x.value);
      this.filterForm.controls['statuses'].patchValue(all);
      event.target.tag = 1;
    } else {
      this.filterForm.controls['statuses'].patchValue([]);
      event.target.tag = 0;
    }
  }

  public clearFilter(): void {
    this.filterForm.reset();
    this.filterForm.controls['statuses'].patchValue([]);
    this.eventList = {
      items: [],
      totalCount: 0,
    };
    this.fetch(true);
  }

  public rowClick(event: IEventListItem): void {
    if (event.status === EventStatus.Started) {
      this.eventService
        .checkAccessViewEventById(event?.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe((isCanView: boolean) =>
          isCanView
            ? this.router.Events.View(event.id)
            : this.router.Events.List().then(() =>
                this.snackService.open(EventErrorMessages.EventNoAccess),
              ),
        );
    } else {
      this.router.Events.View(event.id);
    }
  }

  public getAllEventStatuses(): number[] {
    return Object.keys(EventStatus)
      .filter((k) => !isNaN(Number(k)))
      .map((x) => Number(x));
  }

  private initDefaultSettings(): void {
    this.params.Limit = PageSettingsDefaults.Limit;
    this.isFullListDisplayed = false;
    this.eventList = {
      items: [],
      totalCount: 0,
    };
  }

  private initFormFilter(): void {
    this.filterForm = this.fb.group({
      name: [null],
      startFrom: [null],
      startTo: [null],
      statuses: [[]],
      iAmOwner: [null],
    });
  }

  private fillParamsFromFilter(reloadLimit?: boolean): void {
    if (reloadLimit) {
      this.initDefaultSettings();
    } else {
      this.params.Limit += PageSettingsDefaults.Limit;
    }
    this.params.SortBy = 'name';
    this.params.Filter = new EventFilter();
    this.params.Filter.name = this.filterForm.controls['name'].value
      ? this.filterForm.controls['name'].value
      : null;
    this.params.Filter.startFrom = this.filterForm.controls['startFrom'].value
      ? this.filterForm.controls['startFrom'].value
      : null;
    this.params.Filter.startTo = this.filterForm.controls['startTo'].value
      ? this.filterForm.controls['startTo'].value
      : null;
    this.params.Filter.statuses = this.filterForm.controls['statuses'].value?.length
      ? this.filterForm.controls['statuses'].value
      : null;
    this.params.Filter.excludeOtherUsersDraftedEvents = true;

    if (this.filterForm.value.iAmOwner) {
      const userId: number = this.authService.getUserId();
      if (userId) this.params.Filter.ownerIds = [userId];
    }
  }

  private loadData(params?: GetListParameters<EventFilter>): void {
    this.appStateService.setIsLoadingState(true);
    this.eventsClient
      .getList(params)
      .pipe(
        finalize(() => this.appStateService.setIsLoadingState(false)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res: BaseCollection<IEventListItem>) => {
          this.eventList = res;
          if (this.eventList.items.length === res.totalCount) {
            this.isFullListDisplayed = true;
          }
        },
      });
  }
}
