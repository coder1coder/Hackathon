import {AfterViewInit, Component} from "@angular/core";
import {FormControl, FormGroup} from "@angular/forms";
import {EventService} from "../../../services/event.service";
import {ProblemDetails} from "../../../models/ProblemDetails";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {CreateEvent} from "../../../models/Event/CreateEvent";
import {Actions} from "../../../common/Actions";
import {EventStatus} from "src/app/models/EventStatus";
import {ChangeEventStatusMessage} from "src/app/models/Event/ChangeEventStatusMessage";
import {MatTableDataSource} from "@angular/material/table";
import { MatDialog } from "@angular/material/dialog";
import { EventNewStatusDialog } from "../event-status/event-status.new/event-status.new.component";

@Component({
  selector: 'event-new',
  templateUrl: './event.new.component.html',
  styleUrls: ['./event.new.component.scss']
})

export class EventNewComponent implements AfterViewInit  {

  isLoading: boolean = false;
  filteredEventStatusValues!: (string | EventStatus)[];
  displayedColumns: string[] = ['status', 'message', 'actions'];
  eventStatusDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  eventStatusValues = Object.values(EventStatus).filter(x => !isNaN(Number(x)));

  form = new FormGroup({
    name: new FormControl(''),
    start: new FormControl(this.getEventStartDefault()),
    memberRegistrationMinutes: new FormControl('10'),
    developmentMinutes: new FormControl('10'),
    teamPresentationMinutes: new FormControl('10'),
    maxEventMembers: new FormControl('50'),
    minTeamMembers: new FormControl('2'),
    isCreateTeamsAutomatically: new FormControl(false)
  })

  getEventStartDefault(){
    let now = new Date();
    let offset = (now).getTimezoneOffset() * 60000; //offset in milliseconds
    now.setHours( now.getHours() + 1 );
    now.setMilliseconds(now.getMilliseconds() - offset);
    return (new Date(now.getTime())).toISOString().slice(0, -8);
  }

  ngAfterViewInit(): void {
  }

  constructor(
    private eventService: EventService,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialog: MatDialog) {
  }

  submit(){
    let createEvent = new CreateEvent();

    createEvent.name = this.form.get('name')?.value;
    createEvent.developmentMinutes = this.form.get('developmentMinutes')?.value;
    createEvent.isCreateTeamsAutomatically = this.form.get('isCreateTeamsAutomatically')?.value;
    createEvent.maxEventMembers = this.form.get('maxEventMembers')?.value;
    createEvent.memberRegistrationMinutes = this.form.get('memberRegistrationMinutes')?.value;
    createEvent.minTeamMembers = this.form.get('minTeamMembers')?.value;
    createEvent.start = this.form.get('start')?.value;
    createEvent.teamPresentationMinutes = this.form.get('teamPresentationMinutes')?.value;
    createEvent.changeEventStatusMessages = this.eventStatusDataSource.data;

    this.eventService.create(createEvent)
      .subscribe(_=>{
          this.router.navigate(['/events']);
          this.snackBar.open(`Новое событие добавлено`, Actions.OK, { duration: 4 * 1000 });
        },
        error=>{
          let problemDetails: ProblemDetails = <ProblemDetails>error.error;
          this.snackBar.open(problemDetails.detail,Actions.OK, { duration: 4 * 1000 });
        });
  }

  addStatus() {
    this.filteredEventStatusValues = this.eventStatusValues;

    if(this.eventStatusDataSource.data.length > 0) {
      this.filteredEventStatusValues = this.filteredEventStatusValues.filter(item => this.eventStatusDataSource.data.every(e => item != e.status));
    }

    const createEventNewStatusDialog = this.dialog.open(EventNewStatusDialog, {
      data: {
        statuses: this.filteredEventStatusValues
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

  removeStatus(item: ChangeEventStatusMessage) {
    const index = this.eventStatusDataSource.data.indexOf(item);
    if (index > -1)
      this.eventStatusDataSource.data.splice(index, 1);
    this.eventStatusDataSource.data = this.eventStatusDataSource.data;
  }

  isCanAddStatus() {
    let dataValues = Object.values(this.eventStatusDataSource.data).filter(x => !isNaN(Number(x.status)))
                            .map(x => x.status).sort(function(a, b) { return a - b; });

    return Array.isArray(this.eventStatusValues) &&
           Array.isArray(dataValues) &&
           this.eventStatusValues.length === dataValues.length &&
           this.eventStatusValues.every((val, index) => val === dataValues[index]);
  }

  goBack(){
    history.go(-1)
  }
}
