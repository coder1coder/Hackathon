import {AfterViewInit, Component} from "@angular/core";
import {FormControl, FormGroup} from "@angular/forms";
import {EventService} from "../../../services/event.service";
import {ProblemDetails} from "../../../models/ProblemDetails";
import {MatSnackBar} from "@angular/material/snack-bar";
import {Router} from "@angular/router";
import {CreateEvent} from "../../../models/Event/CreateEvent";
import {Actions} from "../../../common/Actions";

@Component({
  selector: 'event-new',
  templateUrl: './event.new.component.html',
  styleUrls: ['./event.new.component.scss']
})

export class EventNewComponent implements AfterViewInit  {

  isLoading: boolean = false;

  form = new FormGroup({
    name: new FormControl(''),
    start: new FormControl(this.getEventStartDefault()),
    memberRegistrationMinutes: new FormControl('10'),
    developmentMinutes: new FormControl('10'),
    teamPresentationMinutes: new FormControl('10'),
    maxEventMembers: new FormControl('50'),
    minTeamMembers: new FormControl('2'),
    isCreateTeamsAutomatically: new FormControl(false),
    //List<ChangeEventStatusMessage>
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

  constructor(private eventService: EventService, private snackBar: MatSnackBar, private router: Router) {
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

}
