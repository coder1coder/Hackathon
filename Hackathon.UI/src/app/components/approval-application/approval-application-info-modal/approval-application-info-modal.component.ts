import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { IApprovalApplication } from '../../../models/approval-application/approval-application.interface';
import { ApprovalApplicationStatusEnum } from '../../../models/approval-application/approval-application-status.enum';
import { TABLE_DATE_FORMAT } from '../../../common/consts/date-formats';
import {
  CustomDialogComponent,
  ICustomDialogData,
} from '../../custom/custom-dialog/custom-dialog.component';
import { filter, mergeMap, Subject, takeUntil } from 'rxjs';
import { ApplicationApprovalErrorMessages } from '../../../common/error-messages/application-approval-error-messages';
import { ApprovalApplicationRejectModalComponent } from '../approval-application-reject-modal/approval-application-reject-modal.component';
import { ErrorProcessorService } from '../../../services/error-processor.service';
import { SnackService } from '../../../services/snack.service';
import { Router, UrlTree } from '@angular/router';
import { ApprovalApplicationsClient } from 'src/app/clients/approval-applications.client';

@Component({
  selector: 'app-approval-application-info-modal',
  templateUrl: './approval-application-info-modal.component.html',
  styleUrls: ['./approval-application-info-modal.component.scss'],
})
export class ApprovalApplicationInfoModalComponent {
  public tableDateFormat = TABLE_DATE_FORMAT;
  private destroy$ = new Subject();

  constructor(
    public dialogRef: MatDialogRef<ApprovalApplicationInfoModalComponent>,
    @Inject(MAT_DIALOG_DATA) public approvalApplication: IApprovalApplication,
    private dialog: MatDialog,
    private router: Router,
    private approvalApplicationsClient: ApprovalApplicationsClient,
    private errorProcessor: ErrorProcessorService,
    private snackService: SnackService,
  ) {}

  public get approvalApplicationHasActiveStatus(): boolean {
    return (
      this.approvalApplication?.applicationStatus === ApprovalApplicationStatusEnum.Rejected ||
      this.approvalApplication?.applicationStatus === ApprovalApplicationStatusEnum.Approved
    );
  }

  public goToUser(userId: number): void {
    const url: UrlTree = this.router.createUrlTree(['users', `${userId}`]);
    window.open(url.toString(), '_blank');
  }

  public goToEvent(eventId: number): void {
    const url: UrlTree = this.router.createUrlTree(['events', `${eventId}`]);
    window.open(url.toString(), '_blank');
  }

  public approveRequest(): void {
    const data: ICustomDialogData = {
      header: 'Согласование заявки',
      content: `Вы уверены, что хотите согласовать событие:<br>«${this.approvalApplication?.event?.name}»?`,
      acceptButtonText: `Да`,
    };

    this.dialog
      .open(CustomDialogComponent, { data })
      .afterClosed()
      .pipe(
        filter((v) => !!v),
        mergeMap(() =>
          this.approvalApplicationsClient.approveApprovalApplication(this.approvalApplication.id),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.snackService.open(ApplicationApprovalErrorMessages.RequestApproved);
          this.dialogRef.close(true);
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }

  public rejectRequest(): void {
    this.dialog
      .open(ApprovalApplicationRejectModalComponent, { data: this.approvalApplication })
      .afterClosed()
      .pipe(
        filter((v) => !!v),
        mergeMap((comment: string) =>
          this.approvalApplicationsClient.rejectApprovalApplication(
            this.approvalApplication.id,
            comment,
          ),
        ),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => {
          this.snackService.open(ApplicationApprovalErrorMessages.RequestRejected);
          this.dialogRef.close(true);
        },
        error: (error) => this.errorProcessor.Process(error),
      });
  }
}
