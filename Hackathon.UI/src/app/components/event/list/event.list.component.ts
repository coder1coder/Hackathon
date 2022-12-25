import {Component, OnInit, ViewChild} from '@angular/core';
import {EventService} from "../../../services/event.service";
import {BaseCollection} from "../../../models/BaseCollection";
import {EventStatus, EventStatusTranslator} from "../../../models/Event/EventStatus";
import {FormBuilder} from "@angular/forms";
import {EventFilter} from "../../../models/Event/EventFilter";
import {GetListParameters} from "../../../models/GetListParameters";
import * as moment from "moment/moment";
import {RouterService} from "../../../services/router.service";
import {MatSelect} from "@angular/material/select";
import {IEventListItem} from "../../../models/Event/IEventListItem";
import {AuthService} from "../../../services/auth.service";
import {Subject, takeUntil} from "rxjs";
import {map} from "rxjs/operators";
import {PageSettingsDefaults} from "../../../models/PageSettings";
import {DATE_FORMAT} from "../../../common/date-formats";

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss']
})

export class EventListComponent implements OnInit {

  public filterForm = this.fb.group({});
  public eventList: BaseCollection<IEventListItem> = {
    items: [],
    totalCount: 0
  };
  public eventStatusTranslator = EventStatusTranslator;
  public isLoading: boolean = true;
  public isFullListDisplayed: boolean = false;

  private params = new GetListParameters<EventFilter>();
  private static eventImgCount: number = 9;
  private destroy$ = new Subject();

  @ViewChild('statuses') statusesSelect: MatSelect;
  constructor(
    private eventsService: EventService,
    public router: RouterService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
  }

  ngOnInit() {
    this.initDefaultSettings();
    this.initFormFilter();
    this.loadData(this.params);
  }

  public get layoutCssClasses(): string {
    return `container-event hackathon-background ${this.eventList.items.length < 6 ?  'h-100' : ''}`;
  }

  public getDateTimeFormat(date: Date): string {
    return moment(date).local().format(DATE_FORMAT)
  }

  public catchLinkImageError(item: IEventListItem): void {
    item.photoLink = '/assets/img/event-img/event-default.svg';
  }

  public fetch(reloadLimit?: boolean): void {
    this.fillParamsFromFilter(reloadLimit);
    this.loadData(this.params);
  }

  public statusesToggleAll(event: any): void {
    if (event.target.tag == 0 || event.target.tag == undefined) {
      let all = this.statusesSelect?.options.map(x=>x.value);
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
      totalCount: 0
    };
    this.fetch(true);
  }

  public rowClick(event: IEventListItem): void {
    if (event.status == EventStatus.Draft)
      this.router.Events.Edit(event.id);
    else
      this.router.Events.View(event.id);
  }

  public getAllEventStatuses(): number[] {
    return Object
      .keys(EventStatus)
      .filter(k => !isNaN(Number(k)))
      .map(x => Number(x));
  }

  private initDefaultSettings(): void {
    this.params.Limit = PageSettingsDefaults.Limit;
    this.isFullListDisplayed = false;
    this.eventList = {
      items: [],
      totalCount: 0
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

    this.params.Filter = new EventFilter();
    this.params.Filter.name = this.filterForm.controls['name'].value ? this.filterForm.controls['name'].value : null;
    this.params.Filter.startFrom = this.filterForm.controls['startFrom'].value ? this.filterForm.controls['startFrom'].value : null;
    this.params.Filter.startTo = this.filterForm.controls['startTo'].value ? this.filterForm.controls['startTo'].value : null;
    this.params.Filter.statuses = this.filterForm.controls['statuses'].value?.length ? this.filterForm.controls['statuses'].value : null;
    this.params.Filter.excludeOtherUsersDraftedEvents = true;

    if (this.filterForm.value.iAmOwner) {
      const userId = this.authService.getUserId();
      if (userId)
        this.params.Filter.ownerIds = [userId]
    }
  }

  private loadData(params?: GetListParameters<EventFilter>): void {
    this.isLoading = true;
    this.eventsService.getList(params)
      .pipe(
        takeUntil(this.destroy$),
        map((r: BaseCollection<IEventListItem>) => {
          console.log(r.items)
            r.items = EventListComponent.mapPhotoLink(r.items);
            return r;
        })
      )
      .subscribe({
        next: (r: BaseCollection<IEventListItem>) =>  {
          this.eventList = r;
          if (this.eventList.items.length === r.totalCount) {
            this.isFullListDisplayed = true;
          }
        },
        complete: () => this.isLoading = false
      });
  }

  /** Установка дефолтного изображения для ивента */
  private static mapPhotoLink(eventListItems: IEventListItem[]): IEventListItem[] {
    return eventListItems.reduce((acc: IEventListItem[], eventItem: IEventListItem, currentIndex) => {
      if (!Boolean(eventItem.eventImageId)) {
        eventItem.photoLink = EventListComponent.getImageLink(eventListItems.length, currentIndex);
      }
      acc.push(eventItem);
      return acc;
    }, []);
  }

  /** Получить путь к изображению */
  private static getImageLink(arrayLength: number, index: number): string {
    if (index < 0 || arrayLength < 1) return '/assets/img/event-img/event-0.svg';
    if (index <= this.eventImgCount - 1) return `/assets/img/event-img/event-${index}.svg`;
    return `/assets/img/event-img/event-${EventListComponent.recursiveRemoveFraction(index, this.eventImgCount)}.svg`;
  }

  /** Получить валидный индекс изображения */
  private static recursiveRemoveFraction(num: number, divider: number): number {
    let res = num - divider;
    if (res >= divider) return EventListComponent.recursiveRemoveFraction(res, divider);
    return res;
  }
}
