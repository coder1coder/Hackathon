import { Component, OnInit, ViewChild } from '@angular/core';
import { EventService } from "../../../services/event.service";
import { BaseCollectionModel } from "../../../models/BaseCollectionModel";
import { EventStatus, EventStatusTranslator}  from "../../../models/EventStatus";
import { BaseTableListComponent } from "../../BaseTableListComponent";
import { FormBuilder, FormGroup } from "@angular/forms";
import { EventFilterModel } from "../../../models/Event/EventFilterModel";
import { GetFilterModel } from "../../../models/GetFilterModel";
import { EventModel } from 'src/app/models/Event/EventModel';
import * as moment from "moment/moment";
import { RouterService } from "../../../services/router.service";
import { MatSelect } from "@angular/material/select";
import { MatDialog } from '@angular/material/dialog';
import {SnackService} from "../../../services/snack.service";
import {CustomDialog, ICustomDialogData} from "../../custom/custom-dialog/custom-dialog.component";

@Component({
  selector: 'event-list',
  templateUrl: './event.list.component.html',
  styleUrls: ['./event.list.component.scss']
})

export class EventListComponent extends BaseTableListComponent<EventModel> implements OnInit {
  public filterForm: FormGroup = this.fb.group({});
  public eventStatusTranslator = EventStatusTranslator;
  public eventModel = EventModel;
  public moment = moment;

  private dateFormat = 'DD.MM.YYYY HH:mm'
  private eventStatus = EventStatus;

  @ViewChild('statuses') statusesSelect!: MatSelect;

  public override getDisplayColumns(): string[] {
    return ['id', 'name', 'start', 'status', 'user', 'teams', 'members', 'actions'];
  }

  constructor(
    private eventsService: EventService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private snackbar: SnackService,
    public serviceRouter: RouterService
  ) {
    super(EventListComponent.name);
  }

  ngOnInit(): void {
    this.initFilterForm();
  }

  override fetch(){
    let params = new GetFilterModel<EventFilterModel>();

    if (this.pageSettings != undefined)
    {
      params.Page = this.pageSettings.pageIndex+1;
      params.PageSize = this.pageSettings.pageSize;
    }
    params.Filter = new EventFilterModel();
    params.Filter = this.filterForm.value;
    params.Filter.excludeOtherUsersDraftedEvents = true;
    this.eventsService.getAll(params)
      .subscribe({
        next: (r: BaseCollectionModel<EventModel>) =>  {
          this.items = r.items;
          this.pageSettings.length = r.totalCount;
        },
        error: () => {}
      });
  }

  public isEventOwner(event: EventModel): boolean {
    return this.eventsService.isEventOwner(event) && this.eventStatus.Draft === event.status;
  }

  public statusesToggleAll(event:any): void {
    if (event.target.tag == 0 || event.target.tag == undefined)
    {
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
    this.fetch();
  }

  public rowClick(event: EventModel): void {
    this.serviceRouter.Events.View(event.id);
  }

  public getAllEventStatuses(): any[]{
    return Object
      .keys(EventStatus)
      .filter(k => !isNaN(Number(k)))
      .map(x => Number(x));
  }

  public edit(event: EventModel): void {
    this.serviceRouter.Events.Edit(event.id);
  }

  public remove(event: EventModel): void {
    let data: ICustomDialogData = {
      header: 'Удаление события',
      content: `Вы уверены, что хотите удалить событие: ${event.name}?<br> Это действие будет невозможно отменить.`,
      acceptButtonText: `Удалить`,
      acceptButtonColor: `warn`
    };

    this.dialog.open(CustomDialog, { data })
      .afterClosed()
      .subscribe(x => {
        if (x) {
          this.eventsService
            .remove(event.id)
            .subscribe({
              next: () =>  {
                this.snackbar.open(`Событие ${event.name} успешно удалено`);
                this.fetch();
                },
              error: () =>
                this.snackbar.open('Ошибка удаления')
        });
      }
    });
  }

  public dateFormatter(date: Date): string {
    return moment(date).local().format(this.dateFormat)
  }

  private initFilterForm(): void {
    this.filterForm = this.fb.group({
      name: [null],
      startFrom: [null],
      startTo: [null],
      statuses: [null],
    })
  }
}
