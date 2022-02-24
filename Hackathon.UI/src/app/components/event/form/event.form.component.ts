import {AfterViewInit, Component} from "@angular/core";
import {FormControl, FormGroup} from "@angular/forms";
import {EventService} from "../../../services/event.service";
import {ProblemDetails} from "../../../models/ProblemDetails";
import {ActivatedRoute} from "@angular/router";
import {CreateEvent} from "../../../models/Event/CreateEvent";
import {UpdateEvent} from "../../../models/Event/UpdateEvent";
import {EventStatusTranslator, EventStatus} from "src/app/models/EventStatus";
import {ChangeEventStatusMessage} from "src/app/models/Event/ChangeEventStatusMessage";
import {MatTableDataSource} from "@angular/material/table";
import {MatDialog} from "@angular/material/dialog";
import {EventNewStatusDialogComponent} from "../status/event-new-status-dialog.component";
import {SnackService} from "../../../services/snack.service";
import {Observable} from "rxjs";
import * as moment from "moment/moment";
import {EventModel} from "../../../models/Event/EventModel";
import {AuthService} from "../../../services/auth.service";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'event-form',
  templateUrl: './event.form.component.html',
  styleUrls: ['./event.form.component.scss']
})

export class EventFormComponent implements AfterViewInit {

  eventId?:number
  event?:EventModel
  editMode:boolean = false

  isLoading: boolean = false;
  displayedColumns: string[] = ['status', 'message', 'actions'];
  eventStatusDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  eventStatusValues = Object.values(EventStatus).filter(x => !isNaN(Number(x)));
  EventStatusTranslator = EventStatusTranslator;

  form = new FormGroup({
    name: new FormControl(''),
    start: new FormControl(this.getEventStartDefault()),
    memberRegistrationMinutes: new FormControl('10'),
    developmentMinutes: new FormControl('10'),
    teamPresentationMinutes: new FormControl('10'),
    maxEventMembers: new FormControl('50'),
    minTeamMembers: new FormControl('2'),
    isCreateTeamsAutomatically: new FormControl(false),
  })

  constructor(
    private activateRoute: ActivatedRoute,
    private eventService: EventService,
    private authService: AuthService,
    private snackBar: SnackService,
    private router: RouterService,
    private dialog: MatDialog) {
    this.eventId = activateRoute.snapshot.params['eventId'];
    this.editMode = this.eventId !== undefined && !isNaN(Number(this.eventId));
  }

  ngAfterViewInit(): void {

    if (this.editMode)
      this.fetch();

  }

  getEventStartDefault(){
    let now = new Date();
    const offset = new Date().getTimezoneOffset() * 1000 * 60
    now.setHours( now.getHours() + 1 );
    now.setMilliseconds(now.getMilliseconds() - offset);
    return (new Date(now.getTime())).toISOString().slice(0, -8);
  }

  fetch(){

    if (this.eventId == undefined)
      return;

    this.eventService.getById(this.eventId)
      .subscribe(r=>{

          this.event = r;

          this.form = new FormGroup({
            id: new FormControl(r.id),
            name: new FormControl(r.name),
            start: new FormControl(moment(r.start).local().format('yyyy-MM-DDTHH:mm')),
            memberRegistrationMinutes: new FormControl(r.memberRegistrationMinutes),
            developmentMinutes: new FormControl(r.developmentMinutes),
            teamPresentationMinutes: new FormControl(r.teamPresentationMinutes),
            maxEventMembers: new FormControl(r.maxEventMembers),
            minTeamMembers: new FormControl(r.minTeamMembers),
            isCreateTeamsAutomatically: new FormControl(r.isCreateTeamsAutomatically),
            userId: new FormControl(r.userId)
          })

          this.eventStatusDataSource.data = r.changeEventStatusMessages;

        },
        error=>{
          let problemDetails: ProblemDetails = <ProblemDetails>error.error;
          this.snackBar.open(problemDetails.detail);
        });
  }

  isCanPublish(){
    return this.editMode
      && this.event !== undefined
      && this.eventService.isCanPublishEvent(this.event);
  }

  setPublish(){
    this.eventService.setStatus(this.eventId!, EventStatus.Published)
      .subscribe({
        next: (_) =>  {
          this.router.Events.View(this.eventId!).then(_=>
            this.snackBar.open(`Событие опубликовано`))
        },
        error: (err) => {
          let problemDetails: ProblemDetails = <ProblemDetails>err.error;
          this.snackBar.open(problemDetails.detail)}
      });
  }

  submit(){
    let request:Observable<any>;

    if (!this.editMode)
    {
      let event = new CreateEvent();

      event.name = this.form.get('name')?.value;
      event.developmentMinutes = this.form.get('developmentMinutes')?.value;
      event.isCreateTeamsAutomatically = this.form.get('isCreateTeamsAutomatically')?.value;
      event.maxEventMembers = this.form.get('maxEventMembers')?.value;
      event.memberRegistrationMinutes = this.form.get('memberRegistrationMinutes')?.value;
      event.minTeamMembers = this.form.get('minTeamMembers')?.value;
      event.start = this.form.get('start')?.value;
      event.teamPresentationMinutes = this.form.get('teamPresentationMinutes')?.value;

      event.changeEventStatusMessages = this.eventStatusDataSource.data;
      request = this.eventService.create(event)

    } else {
      let event = new UpdateEvent();

      event.id = this.form.get('id')?.value;
      event.name = this.form.get('name')?.value;
      event.developmentMinutes = this.form.get('developmentMinutes')?.value;
      event.isCreateTeamsAutomatically = this.form.get('isCreateTeamsAutomatically')?.value;
      event.maxEventMembers = this.form.get('maxEventMembers')?.value;
      event.memberRegistrationMinutes = this.form.get('memberRegistrationMinutes')?.value;
      event.minTeamMembers = this.form.get('minTeamMembers')?.value;
      event.start = this.form.get('start')?.value;
      event.teamPresentationMinutes = this.form.get('teamPresentationMinutes')?.value;
      event.userId = this.form.get('userId')?.value;

      event.changeEventStatusMessages = this.eventStatusDataSource.data;

      request = this.eventService.update(event)
    }

    request.subscribe({
      next: r=>{

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
      error: err=>{
        let problemDetails: ProblemDetails = <ProblemDetails>err.error;
        this.snackBar.open(problemDetails.detail);
      }
    });
  }

  addStatus() {
    let filteredEventStatusValues = this.eventStatusValues;

    if(this.eventStatusDataSource.data.length > 0) {
      filteredEventStatusValues = filteredEventStatusValues.filter(item => this.eventStatusDataSource.data.every(e => item != e.status));
    }

    const createEventNewStatusDialog = this.dialog.open(EventNewStatusDialogComponent, {
      data: {
        statuses: filteredEventStatusValues
      }
    });

    createEventNewStatusDialog
      .afterClosed()
      .subscribe(result => {
        let changeEventStatusMessage = <ChangeEventStatusMessage> result;
        if(changeEventStatusMessage?.status !== undefined){
          this.eventStatusDataSource.data.push(changeEventStatusMessage);
          this.eventStatusDataSource.data = this.eventStatusDataSource.data;
        }
      });
  }

  getEventStatus(status:EventStatus){
    return EventStatus[status].toLowerCase();
  }

  deleteEvent(){
    this.eventService.remove(this.eventId!)
      .subscribe({
      next: (_) => {
        this.router.Events.List().then(_=>
          this.snackBar.open(`Событие удалено`))
      },
      error: (err) => {
        let problemDetails: ProblemDetails = <ProblemDetails>err.error;
        this.snackBar.open(problemDetails.detail);
      }
    });
  }

  removeStatus(item: ChangeEventStatusMessage) {
    const index = this.eventStatusDataSource.data.indexOf(item);
    if (index > -1)
      this.eventStatusDataSource.data.splice(index, 1);
    this.eventStatusDataSource.data = this.eventStatusDataSource.data;
  }

  isCanDeleteEvent(){
    let userId = this.authService.getUserId();
    return this.eventId != null
      && userId !== null
      && this.event?.userId == userId
  }

  isCanAddStatus() {
    let dataValues = Object.values(this.eventStatusDataSource.data).filter(x => !isNaN(Number(x.status)))
                            .map(x => x.status).sort(function(a, b) { return a - b; });

    return Array.isArray(this.eventStatusValues) &&
           Array.isArray(dataValues) &&
           this.eventStatusValues.length === dataValues.length &&
           this.eventStatusValues.every((val, index) => val === dataValues[index]);
  }
}
