import {AfterViewInit, Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventService} from "../../../services/event.service";
import {EventStatus} from "../../../models/Event/EventStatus";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {Team} from "../../../models/Team/Team";
import {MatTableDataSource} from "@angular/material/table";
import {SnackService} from "../../../services/snack.service";
import {Event} from 'src/app/models/Event/Event';
import {IProblemDetails} from "../../../models/IProblemDetails";
import {RouterService} from "../../../services/router.service";
import {IUser} from "../../../models/User/IUser";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'event-card',
  templateUrl: './event.card.component.html',
  styleUrls: ['./event.card.component.scss']
})
export class EventCardComponent implements AfterViewInit {

  public event: Event = new Event();
  public isLoading: boolean = true;
  public eventTeamsDataSource = new MatTableDataSource<Team>([]);

  private readonly eventId: number = this.activateRoute.snapshot.params['eventId'];
  private membersDataSource: MatTableDataSource<IUser> = new MatTableDataSource<IUser>([]);
  private destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private activateRoute: ActivatedRoute,
    public eventsService: EventService,
    private authService: AuthService,
    private snack: SnackService,
    public router: RouterService
  ) {
  }

  ngAfterViewInit(): void {
    this.fetchEvent();
  }

  public get membersCount(): number {
    return Event.getUsersCount(this.event);
  }

  public get eventTeamsCount(): number {
    return Event.getUsersCount(this.event);
  }

  public get maxEventMembers(): number {
    return this.event?.maxEventMembers
  }

  public get startDate(): string {
    return this.event?.start?.toLocaleString('dd.MM.yyyy, hh:mm z');
  }

  public createNewTeam(): void {
    if (this.event?.status !== EventStatus.Published)
    {
      this.snack.open('Событие должно быть опубликовано')
      return;
    }

    this.router.Teams.New(this.eventId);
  }

  public enterToEvent(): void {
    this.eventsService.join(this.eventId)
      .pipe(takeUntil(this.destroy$))
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

  public leaveFromEvent(): void {
    this.eventsService.leave(this.eventId)
      .pipe(takeUntil(this.destroy$))
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

  public startEvent(): void {
    this.eventsService.setStatus(this.eventId, EventStatus.Started)
      .pipe(takeUntil(this.destroy$))
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

  public finishEvent(): void {
    this.eventsService.setStatus(this.eventId, EventStatus.Finished)
      .pipe(takeUntil(this.destroy$))
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

  private fetchEvent(): void {
    this.isLoading = true;
    this.eventsService.getById(this.eventId)
      .pipe(
        takeUntil(this.destroy$),
        finalize(()=>this.isLoading = false)
      )
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
}
