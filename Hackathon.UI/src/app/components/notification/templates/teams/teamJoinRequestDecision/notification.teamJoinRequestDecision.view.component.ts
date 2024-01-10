import { Component, Input } from '@angular/core';
import { NOTIFICATION_DATETIME_FORMAT } from 'src/app/common/consts/date-formats';
import { ITeamJoinRequestDecisionData } from 'src/app/models/Notification/data/ITeamJoinRequestDecisionData';
import { RouterService } from 'src/app/services/router.service';
import { Notification } from '../../../../../models/Notification/Notification';

@Component({
  selector: 'notification-team-join-request-decision-view',
  templateUrl: './notification.teamJoinRequestDecision.view.component.html',
  styleUrls: ['./notification.teamJoinRequestDecision.view.component.scss'],
})
export class NotificationTeamJoinRequestDecisionViewComponent {
  NOTIFICATION_DATETIME_FORMAT = NOTIFICATION_DATETIME_FORMAT;

  @Input() notify: Notification | undefined;
  @Input() hideDate: boolean = false;

  constructor(public router: RouterService) {}

  get data(): ITeamJoinRequestDecisionData {
    return Notification.getParsedData<ITeamJoinRequestDecisionData>(this.notify.data);
  }
}
