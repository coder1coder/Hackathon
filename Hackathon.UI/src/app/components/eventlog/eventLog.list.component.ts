import { Component } from '@angular/core';
import { BaseTableListComponent } from '../../common/base-components/base-table-list.component';
import { RouterService } from '../../services/router.service';
import { GetListParameters } from '../../models/GetListParameters';
import { IEventLogModel } from '../../models/EventLog/IEventLogModel';
import { BaseCollection } from '../../models/BaseCollection';
import * as moment from 'moment/moment';
import { DATE_FORMAT_DD_MM_YYYY } from 'src/app/common/consts/date-formats';
import { mergeMap, takeUntil } from 'rxjs';
import { fromMobx } from '../../common/functions/from-mobx.function';
import { CurrentUserStore } from '../../shared/stores/current-user.store';
import { LogbookClient } from 'src/app/clients/logbook.client';

@Component({
  selector: 'eventLog-list',
  templateUrl: './eventLog.list.component.html',
  styleUrls: ['./eventLog.list.component.scss'],
})
export class EventLogComponent extends BaseTableListComponent<IEventLogModel> {
  constructor(
    private logbookClient: LogbookClient,
    private routerService: RouterService,
    private currentUserStore: CurrentUserStore,
  ) {
    super(EventLogComponent.name);
    this.currentUserStore.loadCurrentUser();
  }

  public fetch(): void {
    fromMobx(() => this.currentUserStore.currentUser)
      .pipe(
        mergeMap(() => {
          const getFilterModel: GetListParameters<IEventLogModel> =
            new GetListParameters<IEventLogModel>();
          getFilterModel.Offset = this.pageSettings.pageIndex;
          getFilterModel.Limit = this.pageSettings.pageSize;

          return this.logbookClient.getList(getFilterModel);
        }),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: (res: BaseCollection<IEventLogModel>) => {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
        error: () => {},
      });
  }

  public formatDateTime(date: Date): string {
    return moment(date).local().format(DATE_FORMAT_DD_MM_YYYY);
  }

  getDisplayColumns(): string[] {
    return ['description', 'userName', 'timestamp'];
  }

  rowClick(): void {
    /* unused */
  }
}
