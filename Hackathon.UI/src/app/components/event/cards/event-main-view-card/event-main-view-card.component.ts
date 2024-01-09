import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";
import { EventStatusTranslator } from "../../../../models/Event/EventStatus";
import { AuthService } from "../../../../services/auth.service";
import { Team } from "../../../../models/Team/Team";
import { MatTableDataSource } from "@angular/material/table";
import { ChangeEventStatusMessage } from 'src/app/models/Event/ChangeEventStatusMessage';
import { Event } from 'src/app/models/Event/Event';
import { RouterService } from "../../../../services/router.service";
import { KeyValue } from "@angular/common";
import { EventService } from "../../../../services/event/event.service";
import { EventCardBaseComponent } from "../components/event-card-base.component";
import { AppStateService } from "../../../../services/app-state.service";

@Component({
  selector: 'event-event-main-view-card',
  templateUrl: './event-main-view-card.component.html',
  styleUrls: ['./event-main-view-card.component.scss']
})
export class EventMainViewCardComponent extends EventCardBaseComponent implements OnInit {

  public eventStatusesDataSource: MatTableDataSource<ChangeEventStatusMessage> = new MatTableDataSource<ChangeEventStatusMessage>([]);
  public eventTeamsDataSource: MatTableDataSource<Team> = new MatTableDataSource<Team>([]);
  public eventDetails: KeyValue<string, any>[] = [];
  public eventStatusTranslator = EventStatusTranslator;
  public userId: number;

  constructor(
    private activateRoute: ActivatedRoute,
    public eventService: EventService,
    private authService: AuthService,
    public router: RouterService,
    protected appStateService: AppStateService
  ) {
    super(appStateService);
    this.eventId = activateRoute.snapshot.params['eventId'];
    this.userId = authService.getUserId() ?? 0;
  }

  ngOnInit(): void {
    this.initData();
  }

  private initData(): void {
    this.eventDetails = [
      { key: "ID", value: this.event.id },
      { key: "Описание", value: this.event.description },
      { key: "Организатор", value: this.event.owner.userName },
      { key: "Дата начала", value: this.event.start.toLocaleString('dd.MM.yyyy, hh:mm z') },
      { key: "Статус события", value: EventStatusTranslator.Translate(this.event.status ?? -1) },
      { key: "Участники", value: `${Event.getUsersCount(this.event)} / ${this.event?.maxEventMembers}`},
      { key: "Создавать команды автоматически", value: this.event.isCreateTeamsAutomatically ? 'Да' : 'Нет'},
      { key: "Награда / Призовой фонд", value: this.event.award + ' ₽'},
    ]

    this.eventStatusesDataSource.data = this.event.changeEventStatusMessages;
    this.eventTeamsDataSource.data = this.event.teams;
  }

  public getDisplayStatusesColumns(): string[] {
    return ['status', 'message'];
  }
}
