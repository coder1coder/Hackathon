import { Component, Input, inject } from '@angular/core';
import { NOTIFICATION_DATETIME_FORMAT } from 'src/app/common/consts/date-formats';
import { ITeamJoinRequestDecisionData } from 'src/app/models/Notification/data/ITeamJoinRequestDecisionData';
import { RouterService } from 'src/app/services/router.service';
import { Notification } from '../../../../../models/Notification/Notification';
import { DatePipe } from '@angular/common';
import { MatLine } from '@angular/material/grid-list';

@Component({
    selector: 'notification-team-join-request-decision-view',
    templateUrl: './notification.teamJoinRequestDecision.view.component.html',
    styleUrls: ['./notification.teamJoinRequestDecision.view.component.scss'],
    imports: [MatLine, DatePipe]
})
export class NotificationTeamJoinRequestDecisionViewComponent {
  router = inject(RouterService);

  NOTIFICATION_DATETIME_FORMAT = NOTIFICATION_DATETIME_FORMAT;

  @Input() notify: Notification | undefined;
  @Input() hideDate: boolean = false;



  //TODO: remove type assertioon
  get data(): ITeamJoinRequestDecisionData {
    return Notification.getParsedData<ITeamJoinRequestDecisionData>(
      this.notify?.data as string,
    ) as ITeamJoinRequestDecisionData;
  }
}
