import { Event } from "../../../models/Event/Event";
import { AfterViewInit, Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { EventService } from "../../../services/event.service";
import { IProblemDetails } from "../../../models/IProblemDetails";
import { ActivatedRoute } from "@angular/router";
import { ICreateEvent } from "../../../models/Event/ICreateEvent";
import { IUpdateEvent } from "../../../models/Event/IUpdateEvent";
import { EventStatusTranslator, EventStatus } from "src/app/models/EventStatus";
import { ChangeEventStatusMessage } from "src/app/models/Event/ChangeEventStatusMessage";
import { MatTableDataSource } from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { EventNewStatusDialogComponent } from "../status/event-new-status-dialog.component";
import { SnackService } from "../../../services/snack.service";
import { Observable } from "rxjs";
import * as moment from "moment/moment";
import { AuthService } from "../../../services/auth.service";
import { RouterService } from "../../../services/router.service";

@Component({
  selector: 'event-form',
  templateUrl: './event.form.component.html',
  styleUrls: ['./event.form.component.scss']
})

export class EventFormComponent implements OnInit, AfterViewInit {
  public editMode: boolean = false;
  public isLoading: boolean = false;
  public EventStatusTranslator = EventStatusTranslator;
  public displayedColumns: string[] = ['status', 'message', 'actions'];
  public eventStatusDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  public form = new FormGroup({});

  private readonly eventId?: number;
  private event?: Event;
  private dateFormat: string = 'yyyy-MM-DDTHH:mm';
  private eventStatusValues = Object.values(EventStatus).filter(x => !isNaN(Number(x)));
  public minDate = moment(new Date()).format(this.dateFormat).toString();

  constructor(
    private activateRoute: ActivatedRoute,
    private eventService: EventService,
    private authService: AuthService,
    private snackBar: SnackService,
    private router: RouterService,
    private dialog: MatDialog,
    private fb: FormBuilder
    ) {
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

  public isCanPublish(): boolean {
    return this.editMode
      && this.event !== undefined
      && this.eventService.isCanPublishEvent(this.event);
  }

  public setPublish(): void {
    this.eventService.setStatus(this.eventId!, EventStatus.Published)
      .subscribe({
        next: (_) =>  {
          this.router.Events.View(this.eventId!).then(_=>
            this.snackBar.open(`Событие опубликовано`))
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snackBar.open(problemDetails.detail)}
      });
  }

  public submit(): void {
    let request: Observable<any>;

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
      };

      request = this.eventService.create(event);
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
        userId: Number(this.event?.ownerId),
        changeEventStatusMessages: this.eventStatusDataSource.data,
      };

      request = this.eventService.update(event);
    }

    request.subscribe({
      next: r => {
        let eventId = (this.editMode) ? this.eventId : r.id;

        if (this.editMode)
          this.router.Events.Edit(eventId).then(_ =>
            this.snackBar.open(`Событие обновлено`)
          )
        else
          this.router.Events.List().then(_ =>
            this.snackBar.open(`Новое событие добавлено`)
          )
      },
      error: err => {
        let problemDetails: IProblemDetails = <IProblemDetails>err.error;
        this.snackBar.open(problemDetails.detail);
      }
    });
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

  public deleteEvent(): void {
    this.eventService.remove(this.eventId!)
      .subscribe({
      next: (_) => {
        this.router.Events.List().then(_=>
          this.snackBar.open(`Событие удалено`))
      },
      error: (err) => {
        let problemDetails: IProblemDetails = <IProblemDetails>err.error;
        this.snackBar.open(problemDetails.detail);
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

  public isCanDeleteEvent(): boolean {
    let userId = this.authService.getUserId();
    return this.eventId != null
      && userId !== null
      && this.event?.ownerId == userId
  }

  public isCanAddStatus(): boolean {
    let dataValues = Object.values(this.eventStatusDataSource.data).filter(x => !isNaN(Number(x.status)))
                            .map(x => x.status).sort(function(a, b) { return a - b; });

    return Array.isArray(this.eventStatusValues) &&
           Array.isArray(dataValues) &&
           this.eventStatusValues.length === dataValues.length &&
           this.eventStatusValues.every((val, index) => val === dataValues[index]);
  }

  private fetch(): void {
    if (this.eventId == undefined)
      return;

    this.eventService.getById(this.eventId)
      .subscribe((res: Event) => {
        this.event = res;
        this.form.patchValue({
          ...res,
          start: moment(res.start).local().format(this.dateFormat)
         });

        this.eventStatusDataSource.data = res.changeEventStatusMessages;
      },
      (error) => {
        let problemDetails: IProblemDetails = <IProblemDetails>error.error;
        this.snackBar.open(problemDetails.detail);
      });
  }

  private initForm(): void {
    this.form = this.fb.group({
      name: [null],
      start: [EventFormComponent.getEventStartDefault()],
      memberRegistrationMinutes: [10],
      developmentMinutes: [10],
      teamPresentationMinutes: [10],
      maxEventMembers: [50],
      minTeamMembers: [2],
      isCreateTeamsAutomatically: [true]
    });
  }

  private static getEventStartDefault(): string {
    let now = new Date();
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
}
