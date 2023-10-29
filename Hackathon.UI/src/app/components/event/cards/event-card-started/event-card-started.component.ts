import { Component, Injectable, OnInit } from "@angular/core";
import { BehaviorSubject, EMPTY, switchMap, takeUntil } from "rxjs";
import { IProject } from "../../../../models/Project/IProject";
import { EventCardBaseComponent } from "../components/event-card-base.component";
import { AuthService } from "../../../../services/auth.service";
import { Event } from "../../../../models/Event/Event";
import { MatDialog } from "@angular/material/dialog";
import { ProjectClient } from "../../../../clients/project/project.client";
import { ProjectDialog } from "../../../custom/project-dialog/project-dialog.component";
import { SnackService } from "../../../../services/snack.service";
import { ErrorProcessorService } from "../../../../services/error-processor.service";
import { ProjectGitDialog } from "../../../custom/project-git-dialog/project-git-dialog.component";
import { IProjectUpdateFromGitBranch } from "../../../../models/Project/IProjectUpdateFromGitBranch";
import { PagesEnum } from "../../../../models/Event/pages.enum";
import { ChatContextEnum } from "../../../../models/Event/chat-context.enum";

@Component({
  selector: `event-card-started`,
  styleUrls: [`event-card-started.component.scss`],
  templateUrl: `event-card-started.component.html`,
})

@Injectable()
export class EventCardStartedComponent extends EventCardBaseComponent implements OnInit {

  set selectedChatIndex(value) { this._selectedChatIndex.next(value); };
  get selectedChatIndex() { return this._selectedChatIndex.getValue(); }

  public pagesEnum = PagesEnum;
  public eventService = Event;
  public chatContextEnum = ChatContextEnum;
  public selectedPageIndex: number = PagesEnum.Communication;
  public project: IProject | undefined;
  public sideBarWidthPx: number = 200;
  public currentChatId: number = -1;

  private _selectedChatIndex = new BehaviorSubject<number>(0);
  private currentUserId: number | undefined;

  constructor(
    private snackService: SnackService,
    private errorProcessor: ErrorProcessorService,
    private authService:AuthService,
    private projectApiClient: ProjectClient,
    private dialogService: MatDialog,
  ) {
    super();
  }

  public ngOnInit(): void {
    this.currentUserId = this.authService.getUserId();
    this.fetchProject();
    this._selectedChatIndex
      .pipe(takeUntil(this.destroy$))
      .subscribe((value: number) => {
        switch (value){
          case ChatContextEnum.Event:
            this.currentChatId = this.event.id;
            break;
          case ChatContextEnum.Team:
            const team = this.event.teams.find(x=> x.members.find(m=>m.id == this.currentUserId));
            if (team)
              this.currentChatId = team.id;
            break;
        }
    });
  }

  public showProjectDialog(): void {
    this.dialogService.open(ProjectDialog, {
      data: {
        project: this.project
      }
    })
      .afterClosed()
      .pipe(
        switchMap((project: IProject) => {
          if (project?.name) {
            if (project.eventId == 0 || project.teamId == 0) {
              project.eventId = this.event?.id ?? 0;
              project.teamId = this.eventService.getMemberTeam(this.event, this.currentUserId ?? 0)?.id ?? 0;
              return this.projectApiClient.createAsync(project);
            } else {
              return this.projectApiClient.updateAsync(project);
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

    this.dialogService.open(ProjectGitDialog, {
      data: dialogData
    })
      .afterClosed()
      .pipe(
        switchMap((data: IProjectUpdateFromGitBranch) =>  this.projectApiClient.updateProjectFromGitBranch(data)),
        takeUntil(this.destroy$),
      )
      .subscribe({
        next: () => this.fetchProject(),
        error: (errorContext) => this.errorProcessor.Process(errorContext),
      });
  }

  public removeProject(): void {
    this.projectApiClient.remove(this.project?.eventId ?? 0, this.project?.teamId ?? 0)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.fetchProject());
  }

  private fetchProject(): void {
    this.project = null;
    if (!this.currentUserId || !this.event) return;

    const team = this.eventService.getMemberTeam(this.event, this.currentUserId);

    if (!team) return;

    this.projectApiClient.getAsync(this.event.id, team.id)
      .pipe(takeUntil(this.destroy$))
      .subscribe((res: IProject) => this.project = res);
  }
}

