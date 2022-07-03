import {AfterViewInit, Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventService} from "../../../services/event.service";
import {EventStatusTranslator, EventStatus} from "../../../models/Event/EventStatus";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {Team} from "../../../models/Team/Team";
import {MatTableDataSource} from "@angular/material/table";
import {ChangeEventStatusMessage} from 'src/app/models/Event/ChangeEventStatusMessage';
import {SnackService} from "../../../services/snack.service";
import { Event } from 'src/app/models/Event/Event';
import {IProblemDetails} from "../../../models/IProblemDetails";
import {RouterService} from "../../../services/router.service";
import {IUser} from "../../../models/User/IUser";
import {KeyValue} from "@angular/common";

@Component({
  selector: 'event-card',
  templateUrl: './event.card.component.html',
  styleUrls: ['./event.card.component.scss']
})
export class EventCardComponent implements AfterViewInit {

  eventId: number;
  event: Event = new Event();
  EventModel = Event
  isLoading: boolean = true;
  EventStatusTranslator = EventStatusTranslator;
  eventDataSource = new MatTableDataSource<Event>([]);
  eventStatusesDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  eventTeamsDataSource = new MatTableDataSource<Team>([]);
  eventDetails: KeyValue<string,any>[] = [];

  membersDataSource = new MatTableDataSource<IUser>([]);

  selectedTabIndex = 0;

  public userId:number;

  constructor(
    private activateRoute: ActivatedRoute,
    public eventsService: EventService,
    private authService: AuthService,
    private snack: SnackService,
    public router: RouterService) {
    this.eventId = activateRoute.snapshot.params['eventId'];
    this.eventsService = eventsService;
    this.snack = snack;
    this.userId = authService.getUserId() ?? 0;
  }

  ngAfterViewInit(): void {
    this.fetchEvent();
  }

  getMembersCount(){
    return Event.getUsersCount(this.event) / this.event?.maxEventMembers
  }

  getStartDate(){
    return this.event?.start?.toLocaleString('dd.MM.yyyy, hh:mm z');
  }

  getEventStatus(){
    return EventStatusTranslator.Translate(this.event.status ?? -1);
  }

  fetchEvent(){
    this.isLoading = true;
    this.eventsService.getById(this.eventId)
      .pipe(finalize(()=>this.isLoading = false))
      .subscribe({
        next: (r: Event) =>  {
          this.event = r;
          this.eventTeamsDataSource.data = this.event.teams;
          this.membersDataSource.data = this.event.teams
            .map(x => x.members)
            .reduce((x,y) =>
              x?.concat(y));
        },
        error: () => {}
      });
  }

  createNewTeam(){
    if (this.event?.status !== EventStatus.Published)
    {
      this.snack.open('Событие должно быть опубликовано')
      return;
    }

    this.router.Teams.New(this.eventId);
  }

  enterToEvent(){
    this.eventsService.join(this.eventId)
      .subscribe({
        next: (_) => {
          this.snack.open(`Вы зарегистрировались на мероприятие`);
          this.fetchEvent();
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  leaveFromEvent(){
    this.eventsService.leave(this.eventId)
      .subscribe({
        next: (_) => {
          this.snack.open(`Вы покинули мероприятие`);
          this.fetchEvent();
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  startEvent(){
    this.eventsService.setStatus(this.eventId, EventStatus.Started)
      .subscribe({
        next: (_) =>  {
          this.snack.open(`Событие начато`);
          this.fetchEvent();
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  finishEvent(){
    this.eventsService.setStatus(this.eventId, EventStatus.Finished)
      .subscribe({
        next: (_) =>  {
          this.snack.open(`Событие завершено`);
          this.fetchEvent();
        },
        error: (err) => {
          let problemDetails: IProblemDetails = <IProblemDetails>err.error;
          this.snack.open(problemDetails.detail);
        }
      });
  }

  getDisplayStatusesColumns(): string[] {
    return ['status', 'message'];
  }

  getDisplayTeamsColumns(): string[] {
    return ['name', 'members'];
  }
}
