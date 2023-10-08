import {Component} from "@angular/core";
import {BaseTableListComponent} from "../BaseTableListComponent";
import {RouterService} from "../../services/router.service";
import {AuthService} from "../../services/auth.service";
import {GetListParameters} from "../../models/GetListParameters";
import {IEventLogModel} from "../../models/EventLog/IEventLogModel";
import {BaseCollection} from "../../models/BaseCollection";
import {EventLogService} from "../../services/eventLog/eventLog.service";
import * as moment from "moment/moment";
import { DATE_FORMAT } from "src/app/common/date-formats";

@Component({
  selector: 'eventLog-list',
  templateUrl: './eventLog.list.component.html',
  styleUrls: ['./eventLog.list.component.scss']
})

export class EventLogComponent extends BaseTableListComponent<IEventLogModel> {

  constructor(private eventLogService: EventLogService,
              private routerService: RouterService,
              private authService: AuthService) {
    super(EventLogComponent.name);
  }

  fetch(getFilterModel: GetListParameters<IEventLogModel> | undefined): any {

    this.authService.getCurrentUser()?.subscribe(x => {

      let getFilterModel = new GetListParameters<IEventLogModel>();
      getFilterModel.Offset = this.pageSettings.pageIndex;
      getFilterModel.Limit = this.pageSettings.pageSize;

      this.eventLogService.getList(getFilterModel)
        .subscribe({
          next: (r: BaseCollection<IEventLogModel>) =>  {
            this.items = r.items;
            this.pageSettings.length = r.totalCount;
          },
          error: () => {}
        });

    });
  }

  public formatDateTime(date: Date): string {
    return moment(date).local().format(DATE_FORMAT)
  }

  getDisplayColumns(): string[] {
    return ['description', 'userName', 'timestamp'];
  }

  rowClick(item: IEventLogModel): any {
  }
}
