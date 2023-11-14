import { Component, Input } from '@angular/core';
import { ITeamJoinRequestDecisionData } from 'src/app/models/Notification/data/ITeamJoinRequestDecisionData';
import { RouterService } from 'src/app/services/router.service';
import { Notification } from "../../../../../models/Notification/Notification";

@Component({
  selector: 'notification-teamJoinRequestDecision-view',
  templateUrl: './notification.teamJoinRequestDecision.view.component.html',
  styleUrls: ['./notification.teamJoinRequestDecision.view.component.scss'],
})

export class NotificationTeamJoinRequestDecisionViewComponent {
  @Input() notify: Notification | undefined;
  @Input() hideDate: boolean = false;
  
  constructor(public router: RouterService) {
  }

  get data(): ITeamJoinRequestDecisionData
  {
    return (Notification.getParsedData(this.notify.data) as ITeamJoinRequestDecisionData);
  }
}
