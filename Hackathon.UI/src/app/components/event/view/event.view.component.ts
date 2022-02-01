import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute, Router} from "@angular/router";
import {EventModel} from "../../../models/EventModel";
import {EventService} from "../../../services/event.service";
import {EventStatus, TranslatedEventStatus} from "../../../models/EventStatus";
import {Actions} from "../../../common/Actions";
import {finalize} from "rxjs/operators";
import {AuthService} from "../../../services/auth.service";
import {TeamModel} from "../../../models/Team/TeamModel";

@Component({
  selector: 'event-view',
  templateUrl: './event.view.component.html',
  styleUrls: ['./event.view.component.scss']
})
export class EventViewComponent implements AfterViewInit {

  eventId: number;
  event: EventModel | undefined;
  isLoading: boolean = true;

  constructor(
    private activateRoute: ActivatedRoute,
    private eventsService: EventService,
    private authService: AuthService,
    private snackBar: MatSnackBar,
    private router: Router) {

    this.eventId = activateRoute.snapshot.params['eventId'];

    this.eventsService = eventsService;
    this.snackBar = snackBar;
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
          console.log(this.event)
        },
        error: () => {}
      });
  }

  getEventTeams():TeamModel[] | undefined{
    return this.event?.teamEvents?.map(x=>{
      return x.team
    });
  }

  createNewTeam(){

    if (this.event?.status !== EventStatus.Published)
    {
      this.snackBar.open('Событие должно быть опубликовано', Actions.OK, { duration: 4 * 1000 })
      return;
    }

    this.router.navigate(["/teams/new"], { queryParams: { eventId: this.eventId } });
  }

  isCanPublishEvent(){
    return this.event?.status == EventStatus.Draft;
  }

  isCanStartEvent(){
    return this.event?.status == EventStatus.Published;
  }

  isCanAddTeam(){
    return this.event?.status == EventStatus.Published && !this.event?.isCreateTeamsAutomatically
  }

  isCanJoinToEvent() {
    return this.event?.status == EventStatus.Published && !this.isAlreadyInEvent()
  }

  isAlreadyInEvent(){
    if (this.event === undefined)
      return false;

    let userId:number = this.authService.getUserId() ?? 0;

    return this.event!
      .teamEvents?.filter(t => t.team
        .users?.filter(u => u.id == userId)
        .length > 0
      ).length > 0;
  }

  enterToEvent(){
    this.eventsService.join(this.eventId)
      .subscribe({
        next: (_) =>  {
          this.snackBar.open(`Вы зарегистрировались на мероприятии`, Actions.OK, { duration: 4000 });

          this.fetchEvent();
        },
        error: (err) => {
          this.snackBar.open(err.message, Actions.OK, { duration: 4000 });
        }
      });
  }

  setPublished(){
    this.eventsService.setStatus(this.eventId, EventStatus.Published)
      .subscribe({
        next: (_) =>  {
          this.snackBar.open(`Статус успешно изменен`, Actions.OK, { duration: 4000 });
          this.fetchEvent();
        },
        error: (err) => {
          this.snackBar.open(err.message, Actions.OK, { duration: 4000 });
        }
      });
  }

  canStartEvent(){
    return this.event?.status == EventStatus.Published;
  }

  startEvent(){

    this.eventsService.setStatus(this.eventId, EventStatus.Started)
      .subscribe({
        next: (_) =>  {
          this.snackBar.open(`Выполнено`, Actions.OK, { duration: 4000 });
          this.fetchEvent();
        },
        error: (err) => {
          this.snackBar.open(err.message, Actions.OK, { duration: 4000 });
        }
      });
  }

  public getTranslatedEventStatus = (e: EventStatus | undefined) : TranslatedEventStatus => {
    if ( e == 0) {
      return  TranslatedEventStatus.Черновик;}
    if ( e == 1) {
      return  TranslatedEventStatus.Опубликовано;}
    if ( e == 2) {
      return  TranslatedEventStatus.Началось;}
    if ( e == 3) {
      return  TranslatedEventStatus.Разработка;}
    if ( e == 4) {
      return  TranslatedEventStatus.Подготовка;}
    if ( e == 5) {
      return  TranslatedEventStatus.Презентация;}
    if ( e == 6) {
      return  TranslatedEventStatus["Выбор победителя"];}
    if ( e == 7) {
      return  TranslatedEventStatus.Награждение;}
    else
      return  TranslatedEventStatus.Закончено;
  }

  public getTranslatedEventStatusValue(status:TranslatedEventStatus){
    return TranslatedEventStatus[status];
  }
}
