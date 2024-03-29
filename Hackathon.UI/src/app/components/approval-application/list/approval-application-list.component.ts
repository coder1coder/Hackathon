import { Component } from '@angular/core';
import { BaseTableListComponent } from '../../../common/base-components/base-table-list.component';
import {
  IApprovalApplication,
  IApprovalApplicationFilter,
} from '../../../models/approval-application/approval-application.interface';
import { filter, mergeMap, takeUntil } from 'rxjs';
import { GetListParameters } from '../../../models/GetListParameters';
import { BaseCollection } from '../../../models/BaseCollection';
import { TABLE_DATE_FORMAT } from '../../../common/consts/date-formats';
import { ErrorProcessorService } from '../../../services/error-processor.service';
import {
  CustomDialogComponent,
  ICustomDialogData,
} from '../../custom/custom-dialog/custom-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { SnackService } from '../../../services/snack.service';
import { ApplicationApprovalErrorMessages } from '../../../common/error-messages/application-approval-error-messages';
import { ApprovalApplicationRejectModalComponent } from '../approval-application-reject-modal/approval-application-reject-modal.component';
import { ApprovalApplicationStatusEnum } from '../../../models/approval-application/approval-application-status.enum';
import { ApprovalApplicationInfoModalComponent } from '../approval-application-info-modal/approval-application-info-modal.component';
import { ApprovalApplicationsClient } from 'src/app/clients/approval-applications.client';

@Component({
  selector: 'app-approval-applications',
  templateUrl: './approval-application-list.component.html',
  styleUrls: ['./approval-application-list.component.scss'],
})
export class ApprovalApplicationListComponent extends BaseTableListComponent<IApprovalApplication> {
  public tableDateFormat = TABLE_DATE_FORMAT;

  private approvalApplicationFilter: IApprovalApplicationFilter;

  constructor(
    private approvalApplicationsClient: ApprovalApplicationsClient,
    private errorProcessor: ErrorProcessorService,
    private dialog: MatDialog,
    private snackService: SnackService,
  ) {
    super(ApprovalApplicationListComponent.name);
  }

  public getDisplayColumns(): string[] {
    return ['author', 'event', 'requestedAt', 'decisionAt', 'applicationStatus', 'actions'];
  }

  public filterChanged(filterData: IApprovalApplicationFilter): void {
    this.approvalApplicationFilter = filterData;
    this.loadData();
  }

  public approvalApplicationHasActiveStatus(approvalApplication: IApprovalApplication): boolean {
    return (
      approvalApplication?.applicationStatus === ApprovalApplicationStatusEnum.Rejected ||
      approvalApplication?.applicationStatus === ApprovalApplicationStatusEnum.Approved
    );
  }

  private getFilter(): GetListParameters<IApprovalApplicationFilter> {
    const getFilterModel: GetListParameters<IApprovalApplicationFilter> =
      new GetListParameters<IApprovalApplicationFilter>();
    getFilterModel.Offset = this.pageSettings.pageIndex;
    getFilterModel.Limit = this.pageSettings.pageSize;
    getFilterModel.Filter = {
      status: this.approvalApplicationFilter?.status ?? null,
    };
    return getFilterModel;
  }

  override fetch(): void {
    /* Не используется, загрузка данных происходит через фильтр */
  }

  private loadData(): void {
    this.approvalApplicationsClient
      .getApprovalApplicationList(this.getFilter())
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: BaseCollection<IApprovalApplication>) => {
          this.items = res.items;
          this.pageSettings.length = res.totalCount;
        },
        error: () => {},
      });
  }

  public rowClick(approvalApplication: IApprovalApplication): void {
    this.dialog
      .open(ApprovalApplicationInfoModalComponent, {
        data: approvalApplication,
        minWidth: 500,
        maxWidth: 650,
      })
      .beforeClosed()
      .pipe(
        filter((v) => !!v),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.loadData();
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  public approveRequest(approvalApplication: IApprovalApplication): void {
    const data: ICustomDialogData = {
      header: 'Согласование заявки',
      content: `Вы уверены, что хотите согласовать событие:<br>«${approvalApplication?.event?.name}»?`,
      acceptButtonText: `Да`,
    };

    this.dialog
      .open(CustomDialogComponent, { data })
      .afterClosed()
      .pipe(
        filter((v) => !!v),
        mergeMap(() =>
          this.approvalApplicationsClient.approveApprovalApplication(approvalApplication.id),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.snackService.open(ApplicationApprovalErrorMessages.RequestApproved);
          this.loadData();
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  public rejectRequest(approvalApplication: IApprovalApplication): void {
    this.dialog
      .open(ApprovalApplicationRejectModalComponent, { data: approvalApplication })
      .afterClosed()
      .pipe(
        filter((v) => !!v),
        mergeMap((comment: string) =>
          this.approvalApplicationsClient.rejectApprovalApplication(
            approvalApplication.id,
            comment,
          ),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.snackService.open(ApplicationApprovalErrorMessages.RequestRejected);
          this.loadData();
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }
}
