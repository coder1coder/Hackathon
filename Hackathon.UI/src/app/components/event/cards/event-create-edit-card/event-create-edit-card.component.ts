import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from "@angular/core";
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";
import { ActivatedRoute } from "@angular/router";
import { ICreateEvent } from "../../../../models/Event/ICreateEvent";
import { IUpdateEvent } from "../../../../models/Event/IUpdateEvent";
import { EventStatus, EventStatusTranslator } from "src/app/models/Event/EventStatus";
import { ChangeEventStatusMessage } from "src/app/models/Event/ChangeEventStatusMessage";
import { MatTable, MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { EventNewStatusDialogComponent } from "../components/status/event-new-status-dialog.component";
import { SnackService } from "../../../../services/snack.service";
import { delay, Observable, switchMap, takeUntil } from "rxjs";
import * as moment from "moment/moment";
import { AuthService } from "../../../../services/auth.service";
import { RouterService } from "../../../../services/router.service";
import { FileStorageService } from "../../../../services/file-storage.service";
import { SafeUrl } from "@angular/platform-browser";
import { EventErrorMessages } from "src/app/common/error-messages/event-error-messages";
import { EventCardBaseComponent } from "../components/event-card-base.component";
import { EventClient } from "../../../../services/event/event.client";
import { EventService } from "../../../../services/event/event.service";
import { EventStage } from "src/app/models/Event/EventStage";
import {
  EventStageDialogComponent,
  EventStageDialogData
} from "../components/event-stage-dialog/event-stage-dialog.component";
import { CdkDragDrop, moveItemInArray } from "@angular/cdk/drag-drop";
import { IEventTaskItem } from "../../../../models/Event/IEventTaskItem";
import { UploadFileErrorMessages } from "../../../../common/error-messages/upload-file-error-messages";
import { ErrorProcessorService } from "src/app/services/error-processor.service";
import { DATE_FORMAT_DD_MM_YYYY, DATE_FORMAT_YYYY_MM_DD } from "../../../../common/consts/date-formats";
import { checkValue } from "../../../../common/functions/check-value";
import { IEventAgreement } from "../../../../models/Event/IEventAgreement";
import { IBaseCreateResponse } from "../../../../models/IBaseCreateResponse";
import { ApprovalApplicationStatusEnum } from "../../../../models/approval-application/approval-application-status.enum";

@Component({
  selector: 'event-create-edit-card',
  templateUrl: './event-create-edit-card.component.html',
  styleUrls: ['./event-create-edit-card.component.scss'],
})

export class EventCreateEditCardComponent extends EventCardBaseComponent implements OnInit, AfterViewInit {

  @ViewChild('eventStagesTable') eventStagesTable: MatTable<EventStage>;
  @ViewChild('eventTasksTable') eventTasksTable: MatTable<IEventTaskItem>;
  @ViewChild('eventStatusTable') eventStatusTable: MatTable<ChangeEventStatusMessage>;
  @ViewChild('newTaskInput') newTaskInput: ElementRef;

  public editMode: boolean = false;
  public submit: () => void = this.saveForm();
  public eventStatusTranslator = EventStatusTranslator;
  public displayedColumns: string[] = ['status', 'message', 'actions'];
  public eventStatusDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  public eventTasksDataSource = new MatTableDataSource<IEventTaskItem>([]);
  public eventStagesDataSource = new MatTableDataSource<EventStage>([]);
  public form = new FormGroup({});
  public eventImage: SafeUrl;
  public minDate: string;
  public approvalApplicationStatusEnum = ApprovalApplicationStatusEnum;

  private eventStatusValues: (string | EventStatus)[];

  constructor(
    private activateRoute: ActivatedRoute,
    private fileStorageService: FileStorageService,
    private eventHttpService: EventClient,
    public eventService: EventService,
    private authService: AuthService,
    private snackService: SnackService,
    private router: RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder,
    private errorProcessor: ErrorProcessorService,
    ) {
    super();
  }

  ngOnInit(): void {
     this.initVariables();
     this.initForm();
  }

  ngAfterViewInit(): void {
    if (this.editMode) this.fetch();
  }

  public get formValidity(): boolean {
    return this.form.valid && !!this.eventImage;
  }

  public get isValidImage(): boolean {
    return !this.getFormControl('imageId')?.value && (this.form.dirty || this.editMode);
  }

  public getFormControl(controlName: string): FormControl {
    return this.form.get(controlName) as FormControl;
  }

  public getErrorLengthMessage(control: FormControl, length: number): string {
    return control.hasError('minlength') ? `Минимальная длина ${length} символов` : '';
  }

  public getCustomErrorMessage(control: FormControl): string {
    return control.hasError('customError') ? control.getError('customError') : '';
  }

  public saveForm(): () => void {
    return () => {
      const eventStages = this.eventStagesDataSource.data;
      eventStages.forEach((value, index) => {
        value.eventId = this.event?.id;
        value.order = index * 10;
      });

      const eventTasks = this.eventTasksDataSource.data;
      eventTasks.forEach((value, index) => {
        value.order = index * 10;
      });
      const eventData: ICreateEvent | IUpdateEvent = this.editMode ?
        this.eventDataForUpdate(eventStages, eventTasks) :
        this.eventDataForCreate(eventStages, eventTasks);

      const request: Observable<void> | Observable<IBaseCreateResponse> = this.editMode ?
        this.eventHttpService.update(eventData as IUpdateEvent) :
        this.eventHttpService.create(eventData);

      this.applyAgreement(eventData);

      (request as Observable<IBaseCreateResponse>)
        .pipe(
          delay(150),
          takeUntil(this.destroy$),
        )
        .subscribe({
        next: (res) => {
          const eventId = (this.editMode) ? this.eventId : res.id;
          const afterOperationMessage = (this.editMode) ? EventErrorMessages.EventUpdated : EventErrorMessages.EventAdded;
          this.snackService.open(afterOperationMessage);
          if (this.eventStatusTable) {
            this.eventStatusTable.renderRows();
          }
          if (eventId) {
            this.router.Events.View(eventId);
            this.eventService.reloadEvent.emit(true);
          }
        },
        error: errorContext => {
          this.errorProcessor.Process(errorContext);
        }
      });
    }
  }

  public addStatus(): void {
    const filteredEventStatusValues = this.getAvlStatutes();
    this.dialog.open(EventNewStatusDialogComponent, {
      data: {
        statuses: filteredEventStatusValues,
      },
    })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        const changeEventStatusMessage = <ChangeEventStatusMessage> result;
        if (changeEventStatusMessage?.status !== undefined) {
          this.eventStatusDataSource.data.push(changeEventStatusMessage);
          this.eventStatusTable.renderRows();
        }
      });
  }

  public removeStatus(item: ChangeEventStatusMessage): void {
    const index = this.eventStatusDataSource.data.indexOf(item);
    if (index > -1) {
      this.eventStatusDataSource.data.splice(index, 1);
      this.eventStatusTable.renderRows();
    }
  }

  public editStatus(item: ChangeEventStatusMessage): void {
    let filteredEventStatusValues = this.getAvlStatutes();
    filteredEventStatusValues.push(item.status);
    this.dialog.open(EventNewStatusDialogComponent, {
      data: {
        statuses: filteredEventStatusValues,
        editStatus: item,
      }
    })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        if (result) {
          const updateStatusIndex = this.eventStatusDataSource.data.indexOf(item);
          this.eventStatusDataSource.data[updateStatusIndex] = result;
          this.eventStatusTable.renderRows();
        }
      });
  }

  public isCanAddStatus(): boolean {
    const dataValues = Object.values(this.eventStatusDataSource.data).filter(x => !isNaN(Number(x.status)))
                            .map(x => x.status).sort(function(a, b) { return a - b; });

    return Array.isArray(this.eventStatusValues) &&
           Array.isArray(dataValues) &&
           this.eventStatusValues.length === dataValues.length &&
           this.eventStatusValues.every((val, index) => val === dataValues[index]);
  }

  public isCanAddEventStage(): boolean {
    return true;
  }

  public showEventStageCreateView(): void {
    const data = new EventStageDialogData();
    data.eventStages = this.eventStagesDataSource.data;
    data.eventStage = undefined;

    this.dialog.open(EventStageDialogComponent, {
      data: data
    })
      .afterClosed()
      .subscribe(data => {
        const model = <EventStage> data;
        if (model?.name !== undefined) {
          this.eventStagesDataSource.data.push(data);
          this.eventStagesTable.renderRows();
        }
      });
  }

  public updateEventStage(eventStage: EventStage): void {
    const data = new EventStageDialogData();
    data.eventStages = this.eventStagesDataSource.data;
    data.eventStage = eventStage;

    this.dialog.open(EventStageDialogComponent, {
      data: data
    })
      .afterClosed()
      .pipe(takeUntil(this.destroy$))
      .subscribe(result => {
        if (result) {
          const idx = this.eventStagesDataSource.data.indexOf(eventStage);
          this.eventStagesDataSource.data[idx].name = result.name;
          this.eventStagesDataSource.data[idx].duration = result.duration;
          this.eventStagesTable.renderRows();
        }
      });
  }

  public removeEventStage(eventStage: EventStage): void {
    const index = this.eventStagesDataSource.data.indexOf(eventStage);
    if (index > -1) this.eventStagesDataSource.data.splice(index, 1);
    this.eventStagesTable.renderRows();
  }

  public removeEventTask(eventTask: IEventTaskItem): void {
    const index = this.eventTasksDataSource.data.indexOf(eventTask);
    if (index > -1) this.eventTasksDataSource.data.splice(index, 1);
    this.eventTasksTable.renderRows();
  }

  public clearEventImage(): void {
    this.form.controls['fileImage'].reset();
    this.form.controls['imageId'].reset();
    this.eventImage = null;
  }

  public selectEventImage(event: Event): void {
    const target = event.target as HTMLInputElement;
    const files = target.files;

    if (files?.length) {
      this.eventHttpService.setEventImage(files)
        .pipe(
          switchMap((imageId: string) => {
            this.form.controls['imageId'].setValue(imageId);
            return this.fileStorageService.getById(imageId);
          }),
          takeUntil(this.destroy$),
        )
        .subscribe({
          next: (safeUrl: SafeUrl) => this.eventImage = safeUrl,
          error: (err: Error) =>  this.errorProcessor.Process(err, UploadFileErrorMessages.FileUploadError),
        });
    }
  }

  private fetch(): void {
    if (!this.eventId) return;

    this.form.patchValue({
      ...this.event,
      start: moment(this.event.start).local().format(DATE_FORMAT_YYYY_MM_DD),
      agreementRules: checkValue(this.event?.agreement?.rules),
      agreementRequiresConfirmation: this.event?.agreement?.requiresConfirmation,
      imageId: this.event.imageId,
    });

    this.eventStatusDataSource.data = this.event.changeEventStatusMessages;
    this.eventStagesDataSource.data = this.event.stages;

    if (this.event.tasks) {
      this.eventTasksDataSource.data = this.event.tasks;
    }

    if (this.event.imageId) {
      this.fileStorageService.getById(this.event.imageId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (safeUrl: SafeUrl) => this.eventImage = safeUrl,
          error: (err: Error) =>  this.errorProcessor.Process(err, UploadFileErrorMessages.FileUploadError),
        });
    }

    this.form.updateValueAndValidity();
    this.form.controls['start'].markAsTouched();
  }

  private applyAgreement(event: ICreateEvent | IUpdateEvent): void {
    const agreementRules = this.form.get('agreementRules')?.value;
    event.agreement = agreementRules?.length > 0 ?
      {
        id: checkValue(this.event?.agreement?.id),
        rules: agreementRules,
        requiresConfirmation: this.form.get('agreementRequiresConfirmation')?.value,
      } as IEventAgreement
      : null;
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: [null],
      description: [null],
      start: [EventCreateEditCardComponent.getEventStartDefault(), [this.startDateValidator().bind(this)]],
      memberRegistrationMinutes: [10],
      teamPresentationMinutes: [10],
      maxEventMembers: [50],
      minTeamMembers: [2],
      isCreateTeamsAutomatically: [true],
      award: ['0'],
      imageId: [null],
      fileImage: [null],
      agreementRules: [null],
      agreementRequiresConfirmation: [false],
    });
  }

  private static getEventStartDefault(): string {
    const now = new Date();
    const offset = new Date().getTimezoneOffset() * 1000 * 60;
    now.setHours( now.getHours() + 1);
    now.setMilliseconds(now.getMilliseconds() - offset);

    return (new Date(now.getTime())).toISOString().slice(0, -8);
  }

  private getAvlStatutes(): (string | EventStatus)[] {
    let avlStatutes = this.eventStatusValues;
    if (this.eventStatusDataSource.data.length > 0) {
      avlStatutes = avlStatutes.filter(item => this.eventStatusDataSource.data.every(e => item != e.status));
    }

    return avlStatutes;
  }

  public dropEventStageRow(event: CdkDragDrop<MatTableDataSource<EventStage>, EventStage>): void {
    const previousIndex = this.eventStagesDataSource.data.findIndex(row => row === event.item.data);
    moveItemInArray(this.eventStagesDataSource.data, previousIndex, event.currentIndex);
    this.eventStagesTable.renderRows();
  }

  public dropEventTaskRow(event: CdkDragDrop<MatTableDataSource<IEventTaskItem>, IEventTaskItem>): void {
    const previousIndex = this.eventTasksDataSource.data.findIndex(row => row === event.item.data);
    moveItemInArray(this.eventTasksDataSource.data, previousIndex, event.currentIndex);
    this.eventTasksTable.renderRows();
  }

  public addEventTaskFromInput(): void {
    const value = this.newTaskInput.nativeElement.value;

    if (!value) return;
    if (this.eventTasksDataSource.data.find(x=> x.title == value)) return;

    this.eventTasksDataSource.data.push({title: value, order: 0});
    this.eventTasksTable.renderRows();
    this.newTaskInput.nativeElement.value = null;
    this.newTaskInput.nativeElement.focus();
  }

  private initVariables(): void {
    this.eventId = this.activateRoute.snapshot.params['eventId'];
    this.editMode = this.eventId !== undefined && !isNaN(Number(this.eventId));
    this.minDate = moment(new Date()).format(DATE_FORMAT_YYYY_MM_DD).toString();
    this.eventStatusValues = Object.values(EventStatus).filter(x => !isNaN(Number(x)));

    const defaultEventStage = new EventStage();
    defaultEventStage.name = `Основная часть мероприятия`;
    defaultEventStage.duration = 60;
    this.eventStagesDataSource.data.push(defaultEventStage);
  }

  private eventDataForCreate(eventStages: EventStage[], eventTaskItems: IEventTaskItem[]): ICreateEvent {
    return {
      name: checkValue(this.form.get('name')?.value),
      isCreateTeamsAutomatically: checkValue(this.form.get('isCreateTeamsAutomatically')?.value),
      maxEventMembers: checkValue(this.form.get('maxEventMembers')?.value),
      minTeamMembers: this.form.get('minTeamMembers')?.value,
      start: checkValue(this.form.get('start')?.value),
      changeEventStatusMessages: this.eventStatusDataSource.data,
      award: checkValue(this.form.get('award')?.value),
      description: checkValue(this.form.get('description')?.value),
      imageId: checkValue(this.form.get('imageId')?.value),
      stages: eventStages,
      tasks: eventTaskItems,
    };
  }

  private eventDataForUpdate(eventStages: EventStage[], eventTaskItems: IEventTaskItem[]): IUpdateEvent {
    return {
      id: checkValue<number>(this.event?.id),
      name: checkValue(this.form.get('name')?.value),
      isCreateTeamsAutomatically: this.form.get('isCreateTeamsAutomatically')?.value,
      maxEventMembers: checkValue(this.form.get('maxEventMembers')?.value),
      minTeamMembers: checkValue(this.form.get('minTeamMembers')?.value),
      start: checkValue(this.form.get('start')?.value),
      userId: checkValue<number>(this.event?.owner?.id),
      changeEventStatusMessages: this.eventStatusDataSource.data,
      stages: eventStages,
      tasks: eventTaskItems,
      award: checkValue(this.form.get('award')?.value),
      description: checkValue(this.form.get('description')?.value),
      imageId: checkValue(this.form.get('imageId')?.value),
      agreement: checkValue(this.event?.agreement),
    };
  }

  private get currentDate(): moment.Moment {
    return moment(new Date(), DATE_FORMAT_YYYY_MM_DD);
  }

  private startDateValidator(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      if (!control?.value) {
        return null;
      }

      const minDate = this.currentDate;
      if (moment(control.value).isBefore(minDate)) {
        return { customError: `Дата начала мероприятия должна быть позже ${minDate.format(DATE_FORMAT_DD_MM_YYYY)}` };
      }

      return null;
    }
  }
}
