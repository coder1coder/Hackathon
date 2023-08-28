import {Component, OnInit} from '@angular/core';
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {Event} from "../../../../models/Event/Event";
import {RouterService} from "../../../../services/router.service";
import {Team} from "../../../../models/Team/Team";
import {AuthService} from "../../../../services/auth.service";
import {IUser} from "../../../../models/User/IUser";
import {IProject} from "../../../../models/Project/IProject";
import {AppStateService} from "../../../../services/state/app-state.service";

@Component({
  selector: 'app-event-finished-view-card',
  templateUrl: './event-card-finished.component.html',
  styleUrls: ['./event-card-finished.component.scss']
})
export class EventCardFinishedComponent extends EventCardBaseComponent implements OnInit {

  private readonly userId: number;

  constructor(
    public router: RouterService,
    private authService: AuthService,
    private appStateService: AppStateService
  ) {
    super();
    this.userId = authService.getUserId() ?? 0;
  }

  ngOnInit(): void {
    this.appStateService.title = 'Событие: ' + this.event.name;
    this.appStateService.hideTitleBar = true;
    this.appStateService.containerCssClasses = `dark-theme-color`;
  }

  public getEventMembers(): IUser[]{
    return Event.getMembers(this.event);
  }

  public getEventProjects(): IProject[]{
    return [{
      imageId: undefined,
      eventId: 0,
      teamId: 0,
      name: `Система безконтактного приема товара с использованием устройств МАЛИНА-0912`,
      description: `Программное обеспечение для управления системой бесконтактного приема товара с использованием устройств МАЛИНА-0912 от российских производителей`
    },
      {
        imageId: undefined,
        eventId: 0,
        teamId: 0,
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
