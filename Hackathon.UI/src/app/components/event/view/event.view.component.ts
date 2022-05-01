import {AfterViewInit, Component} from '@angular/core';
import {ActivatedRoute} from "@angular/router";
import {EventService} from "../../../services/event.service";
import {EventStatusTranslator, EventStatus} from "../../../models/EventStatus";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {TeamModel} from "../../../models/Team/TeamModel";
import {MatTableDataSource} from "@angular/material/table";
import {ChangeEventStatusMessage} from 'src/app/models/Event/ChangeEventStatusMessage';
import {SnackService} from "../../../services/snack.service";
import { EventModel } from 'src/app/models/Event/EventModel';
import {ProblemDetails} from "../../../models/ProblemDetails";
import {RouterService} from "../../../services/router.service";
import {KeyValuePair} from "../../../common/KeyValuePair";
import {IUserModel} from "../../../models/User/IUserModel";

@Component({
  selector: 'event-view',
  templateUrl: './event.view.component.html',
  styleUrls: ['./event.view.component.scss']
})
export class EventViewComponent implements AfterViewInit {

  eventId: number;
  event: EventModel = new EventModel();
  EventModel = EventModel
  isLoading: boolean = true;
  EventStatusTranslator = EventStatusTranslator;
  eventDataSource = new MatTableDataSource<EventModel>([]);
  eventStatusesDataSource = new MatTableDataSource<ChangeEventStatusMessage>([]);
  eventTeamsDataSource = new MatTableDataSource<TeamModel>([]);
  eventStages: KeyValuePair[] = [];
  eventDetails: KeyValuePair[] = [];

  membersDataSource = new MatTableDataSource<IUserModel>([]);

  selectedTabIndex = 0;

  userId:number;

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

  fetchEvent(){
    this.isLoading = true;
    this.eventsService.getById(this.eventId)
      .pipe(finalize(()=>this.isLoading = false))
      .subscribe({
        next: (r: EventModel) =>  {
          this.event = r;
          this.eventDataSource.data = [this.event];

          this.eventDetails = [
            { key: "ID", value: this.event.id },
            { key: "Название", value: this.event.name },
            { key: "Организатор", value: this.event.user.userName },
            { key: "Дата начала", value: this.event.start.toLocaleString('dd.MM.yyyy, hh:mm z') },
            { key: "Статус события", value: EventStatusTranslator.Translate(this.event.status ?? -1) },
            { key: "Участники", value: EventModel.getUsersCount(this.event) / this.event?.maxEventMembers },
            { key: "Создавать команды автоматически", value: this.event.isCreateTeamsAutomatically ? 'Да' : 'Нет'},
          ]

          this.eventStages = [
            { key: "Регистрация участников", value: this.event.memberRegistrationMinutes + ' мин'},
            { key: "Разработка", value: this.event.developmentMinutes + ' мин'},
            { key: "Презентация", value: this.event.teamPresentationMinutes + ' мин'}
          ];

          this.eventStatusesDataSource.data = this.event.changeEventStatusMessages;
          this.eventTeamsDataSource.data = this.event.teamEvents?.map(x => x.team);
          this.membersDataSource.data = this.event?.teamEvents
            ?.map(x => x.team?.users)
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
          let problemDetails: ProblemDetails = <ProblemDetails>err.error;
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
          let problemDetails: ProblemDetails = <ProblemDetails>err.error;
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
          let problemDetails: ProblemDetails = <ProblemDetails>err.error;
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
          let problemDetails: ProblemDetails = <ProblemDetails>err.error;
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
