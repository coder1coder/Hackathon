import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, EMPTY, filter, switchMap, takeUntil } from 'rxjs';
import { IProject } from '../../../../models/Project/IProject';
import { EventCardBaseComponent } from '../components/event-card-base.component';
import { AuthService } from '../../../../services/auth.service';
import { Event } from '../../../../models/Event/Event';
import { MatDialog } from '@angular/material/dialog';
import { ProjectDialogComponent } from '../../../custom/project-dialog/project-dialog.component';
import { ErrorProcessorService } from '../../../../services/error-processor.service';
import { ProjectGitDialogComponent } from '../../../custom/project-git-dialog/project-git-dialog.component';
import { IProjectUpdateFromGitBranch } from '../../../../models/Project/IProjectUpdateFromGitBranch';
import { PagesEnum } from '../../../../models/Event/pages.enum';
import { ChatContextEnum } from '../../../../models/Event/chat-context.enum';
import { Team } from '../../../../models/Team/Team';
import { IUser } from '../../../../models/User/IUser';
import { SignalRService } from 'src/app/services/signalr.service';
import { AppStateService } from '../../../../services/app-state.service';
import { ProjectsClient } from 'src/app/clients/projects.client';

@Component({
  selector: `event-card-started`,
  styleUrls: [`event-card-started.component.scss`],
  templateUrl: `event-card-started.component.html`,
})
export class EventCardStartedComponent extends EventCardBaseComponent implements OnInit {
  public set selectedChatIndex(value) {
    this._selectedChatIndex.next(value);
  }
  public get selectedChatIndex(): number {
    return this._selectedChatIndex.getValue();
  }

  public pagesEnum = PagesEnum;
  public Event = Event;
  public chatContextEnum = ChatContextEnum;
  public selectedPageIndex: number = PagesEnum.Communication;
  public project: IProject;
  public currentChatId: number = -1;
  public currentStageName: string | undefined;

  private _selectedChatIndex = new BehaviorSubject<number>(0);
  private currentUserId: number;

  constructor(
    private errorProcessor: ErrorProcessorService,
    private authService: AuthService,
    private projectsClient: ProjectsClient,
    private dialogService: MatDialog,
    private signalRService: SignalRService,
    protected appStateService: AppStateService,
  ) {
    super(appStateService);
    signalRService.onEventStageChanged = (integrationEvent): void => {
      if (integrationEvent.eventId === this.event.id) {
        this.setEventStageName(integrationEvent.eventStageId);
      }
    };
  }

  public ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.setEventStageName();

    this.fetchProject();
    this._selectedChatIndex.pipe(takeUntil(this.destroy$)).subscribe((value: number) => {
      switch (value) {
        case ChatContextEnum.Event:
          this.currentChatId = this.event.id;
          break;
        case ChatContextEnum.Team:
          const team: Team = this.event.teams.find((team: Team) =>
            team.members.find((member: IUser) => member.id === this.currentUserId),
          );
          if (team) this.currentChatId = team.id;
          break;
      }
    });
  }

  private setEventStageName(eventStageId: number | null = null): void {
    this.currentStageName = Event.getStageName(
      this.event,
      eventStageId ?? this.event.currentStageId,
    );
  }

  public showProjectDialog(): void {
    this.dialogService
      .open(ProjectDialogComponent, {
        data: {
          project: this.project,
        },
      })
      .afterClosed()
      .pipe(
        switchMap((project: IProject) => {
          if (project?.name) {
            if (project.eventId === 0 || project.teamId === 0) {
              project.eventId = this.event?.id ?? 0;
              project.teamId =
                this.Event.getMemberTeam(this.event, this.currentUserId ?? 0)?.id ?? 0;
              return this.projectsClient.createAsync(project);
            } else {
              return this.projectsClient.updateAsync(project);
            }
          } else {
            return EMPTY;
          }
        }),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => this.fetchProject(),
        error: (errorContext) => this.errorProcessor.Process(errorContext),
      });
  }

  public showUpdateProjectFromBitBranchDialog(): void {
    const dialogData: IProjectUpdateFromGitBranch = {
      eventId: this.project?.eventId,
      teamId: this.project?.teamId,
      linkToGitBranch: this.project?.linkToGitBranch,
    };

    this.dialogService
      .open(ProjectGitDialogComponent, {
        data: dialogData,
      })
      .afterClosed()
      .pipe(
        filter((data: IProjectUpdateFromGitBranch) => data !== undefined),
        switchMap((data: IProjectUpdateFromGitBranch) =>
          this.projectsClient.updateProjectFromGitBranch(data)
        ),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => this.fetchProject(),
        error: (errorContext) => this.errorProcessor.Process(errorContext),
      });
  }

  public removeProject(): void {
    this.projectsClient
      .remove(this.project?.eventId ?? 0, this.project?.teamId ?? 0)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.fetchProject());
  }

  private fetchProject(): void {
    this.project = null;
    if (!this.currentUserId || !this.event) return;

    const team: Team = this.Event.getMemberTeam(this.event, this.currentUserId);
    if (!team) return;

    this.projectsClient
      .getAsync(this.event.id, team.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (res: IProject) => (this.project = res),
        error: () => {},
      });
  }
}
