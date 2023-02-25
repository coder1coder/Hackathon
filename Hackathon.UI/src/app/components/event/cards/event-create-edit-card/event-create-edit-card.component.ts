import {AfterViewInit, Component, OnInit, ViewChild} from "@angular/core";
import {FormBuilder, FormGroup} from "@angular/forms";
import {IProblemDetails} from "../../../../models/IProblemDetails";
import {ActivatedRoute} from "@angular/router";
import {ICreateEvent} from "../../../../models/Event/ICreateEvent";
import {IUpdateEvent} from "../../../../models/Event/IUpdateEvent";
import {EventStatus, EventStatusTranslator} from "src/app/models/Event/EventStatus";
import {ChangeEventStatusMessage} from "src/app/models/Event/ChangeEventStatusMessage";
import {MatTable, MatTableDataSource} from "@angular/material/table";
import {MatDialog} from "@angular/material/dialog";
import {EventNewStatusDialogComponent} from "../components/status/event-new-status-dialog.component";
import {SnackService} from "../../../../services/snack.service";
import {Observable} from "rxjs";
import * as moment from "moment/moment";
import {AuthService} from "../../../../services/auth.service";
import {RouterService} from "../../../../services/router.service";
import {FileUtils} from "../../../../common/FileUtils";
import {FileStorageService} from "../../../../services/file-storage.service";
import {Bucket} from "../../../../common/Bucket";
import {IStorageFile} from "../../../../models/FileStorage/IStorageFile";
import {SafeUrl} from "@angular/platform-browser";
import {EventErrorMessages} from "src/app/common/EventErrorMessages";
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {EventHttpService} from "../../../../services/event/event.http-service";
import {EventService} from "../../../../services/event/event.service";
import { EventStage } from "src/app/models/Event/EventStage";
import {
  EventStageDialogComponent,
  EventStageDialogData
} from "../components/event-stage-dialog/event-stage-dialog.component";
import {CdkDragDrop, moveItemInArray} from "@angular/cdk/drag-drop";

@Component({
  selector: 'event-create-edit-card',
  templateUrl: './event-create-edit-card.component.html',
  styleUrls: ['./event-create-edit-card.component.scss']
})

export class EventCreateEditCardComponent extends EventCardBaseComponent implements OnInit, AfterViewInit {

  private dateFormat: string = 'yyyy-MM-DDTHH:mm';
  private eventStatusValues = Object.values(EventStatus).filter(x => !isNaN(Number(x)));

  public editMode: boolean = false;
  public submit: () => void = this.saveForm();
  public EventStatusTranslator = EventStatusTranslator;
  public displayedColumns: string[] = ['status', 'message', 'actions'];
  public eventStatusDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);

  @ViewChild('eventStagesTable') eventStagesTable: MatTable<any>;
  public eventStagesDataSource = new MatTableDataSource<EventStage>([]);

  public form = new FormGroup({});
  public eventImage: any;
  public minDate = moment(new Date()).format(this.dateFormat).toString();

  constructor(
    private activateRoute: ActivatedRoute,
    private fileStorageService: FileStorageService,
    private eventHttpService: EventHttpService,
    public eventService: EventService,
    private authService: AuthService,
    private snackService: SnackService,
    private router: RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder
    ) {
    super();
    this.eventId = activateRoute.snapshot.params['eventId'];
    this.editMode = this.eventId !== undefined && !isNaN(Number(this.eventId));
  }

  ngOnInit(): void {
     this.initForm();
  }

  ngAfterViewInit(): void {
    if (this.editMode)
      this.fetch();
  }

  public saveForm(): () => void {
    return () => {
      let request: Observable<any>;

      let eventStages = this.eventStagesDataSource.data;
      eventStages.forEach((value, index) => {
        value.eventId = this.event?.id;
        value.order = index * 10;
      });

      if (!this.editMode) {
        let event: ICreateEvent = {
          name: this.form.get('name')?.value,
          developmentMinutes: this.form.get('developmentMinutes')?.value,
          isCreateTeamsAutomatically: this.form.get('isCreateTeamsAutomatically')?.value,
          maxEventMembers: this.form.get('maxEventMembers')?.value,
          memberRegistrationMinutes: this.form.get('memberRegistrationMinutes')?.value,
          minTeamMembers: this.form.get('minTeamMembers')?.value,
          start: this.form.get('start')?.value,
          teamPresentationMinutes: this.form.get('teamPresentationMinutes')?.value,
          changeEventStatusMessages: this.eventStatusDataSource.data,
          award: this.form.get('award')?.value,
          description: this.form.get('description')?.value,
          imageId: this.form.get('imageId')?.value,
          stages: eventStages,
          rules: this.form.get('rules')?.value
        };

        request = this.eventHttpService.create(event);
      } else {
        let event: IUpdateEvent = {
          id: Number(this.event?.id),
          name: this.form.get('name')?.value,
          developmentMinutes: this.form.get('developmentMinutes')?.value,
          isCreateTeamsAutomatically: this.form.get('isCreateTeamsAutomatically')?.value,
          maxEventMembers: this.form.get('maxEventMembers')?.value,
          memberRegistrationMinutes: this.form.get('memberRegistrationMinutes')?.value,
          minTeamMembers: this.form.get('minTeamMembers')?.value,
          start: this.form.get('start')?.value,
          teamPresentationMinutes: this.form.get('teamPresentationMinutes')?.value,
          userId: Number(this.event?.owner?.id),
          changeEventStatusMessages: this.eventStatusDataSource.data,
          stages: eventStages,
          award: this.form.get('award')?.value,
          description: this.form.get('description')?.value,
          imageId: this.form.get('imageId')?.value,
          rules: this.form.get('rules')?.value
        };

        request = this.eventHttpService.update(event);
      }

      request.subscribe({
        next: r => {
          const eventId = (this.editMode) ? this.eventId : r.id;
          const afterOperationMessage = (this.editMode) ? EventErrorMessages.EventUpdated : EventErrorMessages.EventAdded;
          this.eventService.reloadEvent.emit(true);
          this.snackService.open(afterOperationMessage);

          if (eventId) {
            this.router.Events.View(eventId);
          }
        },
        error: err => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackService.open(problemDetails.detail);
        }
      });
    }
  }

  public addStatus(): void {
    let filteredEventStatusValues = this.getAvlStatutes();
    this.dialog.open(EventNewStatusDialogComponent, {
      data: {
        statuses: filteredEventStatusValues
      }
    })
    .afterClosed()
    .subscribe(result => {
      let changeEventStatusMessage = <ChangeEventStatusMessage> result;
      if(changeEventStatusMessage?.status !== undefined){
        this.eventStatusDataSource.data.push(changeEventStatusMessage);
        this.eventStatusDataSource.data = this.eventStatusDataSource.data;
      }
    });
  }

  public removeStatus(item: ChangeEventStatusMessage): void {
    const index = this.eventStatusDataSource.data.indexOf(item);
    if (index > -1)
      this.eventStatusDataSource.data.splice(index, 1);
      this.eventStatusDataSource.data = this.eventStatusDataSource.data;
  }

  public editStatus(item: ChangeEventStatusMessage): void {
    let filteredEventStatusValues = this.getAvlStatutes();
    filteredEventStatusValues.push(item.status);
    this.dialog.open(EventNewStatusDialogComponent, {
      data: {
        statuses: filteredEventStatusValues,
        editStatus: item
      }
    })
    .afterClosed()
    .subscribe(result => {
      if(result) {
        let updateStatusIndex = this.eventStatusDataSource.data.indexOf(item);
        this.eventStatusDataSource.data[updateStatusIndex] = result;
        this.eventStatusDataSource.data = this.eventStatusDataSource.data;
      }
    });
  }

  public isCanAddStatus(): boolean {
    let dataValues = Object.values(this.eventStatusDataSource.data).filter(x => !isNaN(Number(x.status)))
                            .map(x => x.status).sort(function(a, b) { return a - b; });

    return Array.isArray(this.eventStatusValues) &&
           Array.isArray(dataValues) &&
           this.eventStatusValues.length === dataValues.length &&
           this.eventStatusValues.every((val, index) => val === dataValues[index]);
  }

  public isCanAddEventStage():boolean {
    return true;
  }

  public showEventStageCreateView():void{

    let data = new EventStageDialogData();
    data.eventStages = this.eventStagesDataSource.data;
    data.eventStage = undefined;

    this.dialog.open(EventStageDialogComponent, {
      data: data
    })
      .afterClosed()
      .subscribe(data => {
        let model = <EventStage> data;
        if(model?.name !== undefined){
          this.eventStagesDataSource.data.push(data);
          this.eventStagesDataSource.data = this.eventStagesDataSource.data;
        }
      });
  }

  public updateEventStage(eventStage: EventStage):void {

    let data = new EventStageDialogData();
    data.eventStages = this.eventStagesDataSource.data;
    data.eventStage = eventStage;

    this.dialog.open(EventStageDialogComponent, {
      data: data
    })
      .afterClosed()
      .subscribe(result => {
        if (result) {
          let idx = this.eventStagesDataSource.data.indexOf(eventStage);

          this.eventStagesDataSource.data[idx].name = result.name;
          this.eventStagesDataSource.data[idx].duration = result.duration;

          this.eventStagesDataSource.data = this.eventStagesDataSource.data;
        }
      });
  }

  public removeEventStage(eventStage: EventStage):void {
    const index = this.eventStagesDataSource.data.indexOf(eventStage);
    if (index > -1) this.eventStagesDataSource.data.splice(index, 1);
    this.eventStagesDataSource.data = this.eventStagesDataSource.data;
  }

  public clearEventImage(): void{
    this.form.controls['imageId'].reset();
    this.eventImage = undefined;
  }

  public selectEventImage(e: any): void{

    if ( !(e?.target?.files?.length > 0) )
      return;

    let file:File = e.target.files[0];

    if (!FileUtils.IsImage(file)) {
      this.snackService.open(EventErrorMessages.FileIsNotImage);
      return;
    }

    if (file.size / FileUtils.Divider > FileUtils.MaxFileSize) {
      this.snackService.open(EventErrorMessages.FileSizeOutOfRange);
      return;
    }

    this.fileStorageService
      .upload(Bucket.Events, file)
      .subscribe({
        next: (r : IStorageFile) =>{
          this.form.controls['imageId'].setValue(r.id);
          this.loadImage(r.id);
        },
        error: () => {
          this.snackService.open(EventErrorMessages.FileUploadError)
        }});
  }

  private loadImage(fileId:string){
    this.fileStorageService.getById(fileId)
      .subscribe({
        next: (safeUrl: SafeUrl) => this.eventImage = safeUrl,
        error: _ => this.snackService.open(EventErrorMessages.FileUploadError)});
  }

  private fetch(): void {
    if (this.eventId == undefined)
      return;

    this.form.patchValue({
      ...this.event,
      start: moment(this.event.start).local().format(this.dateFormat)
    });

    this.eventStatusDataSource.data = this.event.changeEventStatusMessages;
    this.eventStagesDataSource.data = this.event.stages;

    if (this.event.imageId)  {
      this.loadImage(this.event.imageId);
    }
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: [null],
      description: [null],
      start: [EventCreateEditCardComponent.getEventStartDefault()],
      memberRegistrationMinutes: [10],
      developmentMinutes: [10],
      teamPresentationMinutes: [10],
      maxEventMembers: [50],
      minTeamMembers: [2],
      isCreateTeamsAutomatically: [true],
      award: ['0'],
      imageId: [null],
      rules: [null]
    });
  }

  private static getEventStartDefault(): string {
    const now = new Date();
    const offset = new Date().getTimezoneOffset() * 1000 * 60
    now.setHours( now.getHours() + 1 );
    now.setMilliseconds(now.getMilliseconds() - offset);
    return (new Date(now.getTime())).toISOString().slice(0, -8);
  }

  private getAvlStatutes(): (string | EventStatus)[] {
    let avlStatutes = this.eventStatusValues;
    if(this.eventStatusDataSource.data.length > 0) {
      avlStatutes = avlStatutes.filter(item => this.eventStatusDataSource.data.every(e => item != e.status));
    }

    return avlStatutes;
  }

  public dropEventStageRow(event: CdkDragDrop<MatTableDataSource<EventStage>, EventStage>) {
    const previousIndex = this.eventStagesDataSource.data.findIndex(row => row === event.item.data);
    moveItemInArray(this.eventStagesDataSource.data, previousIndex, event.currentIndex);
    this.eventStagesTable.renderRows();
  }
}