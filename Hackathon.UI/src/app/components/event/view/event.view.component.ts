import {AfterViewInit, Component} from '@angular/core';
import {MatSnackBar} from "@angular/material/snack-bar";
import {ActivatedRoute} from "@angular/router";
import {EventModel} from "../../../models/EventModel";
import {EventService} from "../../../services/event.service";
import {MatDialog} from "@angular/material/dialog";
import {TeamNewComponent} from "../../team/new/team.new.component";
import {EventStatus} from "../../../models/EventStatus";
import {Actions} from "../../../common/Actions";
import {finalize} from "rxjs/operators";

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
    private snackBar: MatSnackBar,
    private dialog: MatDialog) {

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
        },
        error: () => {}
      });
  }

  createNewTeam(){

    if (this.event?.status !== EventStatus.Published)
    {
      this.snackBar.open('Событие должно быть опубликовано', Actions.OK, { duration: 4 * 1000 })
      return;
    }

    const createNewTeamDialog = this.dialog.open(TeamNewComponent, {
      data: {
        eventId: this.eventId
      },
    });

    createNewTeamDialog.afterClosed().subscribe(r => {

      if (r === true)
        this.fetchEvent();
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
}
