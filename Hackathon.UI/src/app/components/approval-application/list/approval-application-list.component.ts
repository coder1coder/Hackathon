import { Component } from '@angular/core';
import { RouterService } from "../../../services/router.service";
import { ApprovalApplicationsService } from "../../../services/approval-applications/approval-applications.service";
import { BaseTableListComponent} from "../../../common/base-components/base-table-list.component";
import { IApprovalApplication } from "../../../models/approval-application/approval-application.interface";
import { UserFilter } from "../../../models/User/UserFilter";
import { takeUntil } from "rxjs";
import { GetListParameters } from "../../../models/GetListParameters";
import { BaseCollection } from "../../../models/BaseCollection";

@Component({
  selector: 'app-approval-applications',
  templateUrl: './approval-application-list.component.html',
  styleUrls: ['./approval-application-list.component.scss'],
})
export class ApprovalApplicationListComponent extends BaseTableListComponent<IApprovalApplication> {

  constructor(
    private approvalApplicationsService: ApprovalApplicationsService,
    private routerService: RouterService,
  ) {
    super(ApprovalApplicationListComponent.name);
  }

  public getDisplayColumns(): string[] {
    return ['ApprovalApplicationStatus', 'email', 'fullName', 'actions'];
  }

  override fetch(): void {
    const userFilter = new UserFilter();
    let getFilterModel = new GetListParameters<UserFilter>();
    getFilterModel.Offset = this.pageSettings.pageIndex;
    getFilterModel.Limit = this.pageSettings.pageSize;
    getFilterModel.Filter = userFilter;

    this.approvalApplicationsService.getApprovalApplicationList(getFilterModel)
     .pipe(takeUntil(this.destroy$))
      .subscribe(({
        next: (res: BaseCollection<IApprovalApplication>) =>  {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
        error: () => {}
      }))
  }

  public rowClick(approvalApplication: IApprovalApplication): void {
    console.log('approvalApplication', approvalApplication)
  }
}
