import {Component, OnInit} from '@angular/core';
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {Event} from "../../../../models/Event/Event";
import {DATE_FORMAT} from "../../../../common/date-formats";
import * as moment from "moment/moment";
import {RouterService} from "../../../../services/router.service";
import {Team} from "../../../../models/Team/Team";
import {AuthService} from "../../../../services/auth.service";
import {IUser} from "../../../../models/User/IUser";
import {IProject} from "../../../../models/Project/IProject";

@Component({
  selector: 'app-event-finished-view-card',
  templateUrl: './event-finished-view-card.component.html',
  styleUrls: ['./event-finished-view-card.component.scss']
})
export class EventFinishedViewCardComponent extends EventCardBaseComponent implements OnInit {

  private readonly userId: number;

  constructor(
    public router: RouterService,
    private authService: AuthService
  ) {
    super();
    this.userId = authService.getUserId() ?? 0;
  }

  ngOnInit(): void {
  }

  public get startDate(): string {
    return moment(this.event?.start).local().format(DATE_FORMAT)
  }

  public getEventMembers(): IUser[]{
    return Event.getMembers(this.event);
  }

  public getEventProjects(): IProject[]{
    return [{
      imageId: undefined,
      name: `Система безконтактного приема товара с использованием устройств МАЛИНА-0912`,
      description: `Программное обеспечение для управления системой бесконтактного приема товара с использованием устройств МАЛИНА-0912 от российских производителей`
    },
      {
        imageId: undefined,
        name: `Мобильное приложения для контроля региональных поставок `,
        description: `Мобильное приложение под ОС Android позволяет контролировать логистику товаров на региональном уровне`
      }];
  }

  public goToTeamCard = (item: Team) => {
    if (item.owner?.id === this.userId) {
      this.router.Teams.MyTeam();
    } else {
      this.router.Teams.View(item.id);
    }
  };
}
