import {Component, Injectable, OnInit} from "@angular/core";
import {BehaviorSubject, Observable} from "rxjs";
import {IProject} from "../../../../models/Project/IProject";
import {EventCardBaseComponent} from "../components/event-card-base.component";
import {AuthService} from "../../../../services/auth.service";
import {Event} from "../../../../models/Event/Event";
import {MatDialog} from "@angular/material/dialog";
import {ProjectClient} from "../../../../clients/project/project.client";
import {ProjectDialog} from "../../../custom/project-dialog/project-dialog.component";
import {SnackService} from "../../../../services/snack.service";
import {ErrorProcessor} from "../../../../services/errorProcessor";
import {ProjectGitDialog} from "../../../custom/project-git-dialog/project-git-dialog.component";
import {IProjectUpdateFromGitBranch} from "../../../../models/Project/IProjectUpdateFromGitBranch";
import {AppStateService} from "../../../../services/state/app-state.service";

@Component({
  selector: `event-card-started`,
  styleUrls: [`event-card-started.component.scss`],
  templateUrl: `event-card-started.component.html`
})

@Injectable()
export class EventCardStartedComponent extends EventCardBaseComponent implements OnInit
{
  Pages = Pages;
  Event = Event;
  ChatContext = ChatContext;

  set selectedChatIndex(value) { this._selectedChatIndex.next(value); };
  get selectedChatIndex() { return this._selectedChatIndex.getValue(); }
  private _selectedChatIndex = new BehaviorSubject<number>(0);

  currentUserId: number | undefined;
  selectedPageIndex: number = Pages.Communication;
  project: IProject | undefined;
  sideBarWidthPx: number = 200;
  currentChatId: number = -1;

  constructor(
    private snackService: SnackService,
    private errorProcessor: ErrorProcessor,
    private authService:AuthService,
    private projectApiClient: ProjectClient,
    private dialogService: MatDialog,
    private appStateService: AppStateService) {
    super();
  }

  ngOnInit(): void {

    this.appStateService.hideTitleBar = true;
    this.appStateService.logoMinWidth =(this.sideBarWidthPx - 16).toString();
    this.appStateService.containerCssClasses = `relative`;

    this.currentUserId = this.authService.getUserId();
    this.fetchProject();
    this._selectedChatIndex.subscribe((value:number) =>{
      switch (value){
        case ChatContext.Event:

          this.currentChatId = this.event.id;

          break;

        case ChatContext.Team:

          let team = this.event.teams.find(x=> x.members.find(m=>m.id == this.currentUserId));

          if (team)
            this.currentChatId = team.id;

          break;
      }
    })
    }

  showProjectDialog() {

    this.dialogService.open(ProjectDialog, {
      data: {
        project: this.project
      }
    })
      .afterClosed()
      .subscribe((project: IProject) => {

        if(project?.name !== undefined){

          let action: Observable<object>;

          if (project.eventId == 0 || project.teamId == 0)
          {
            project.eventId = this.event?.id ?? 0;
            project.teamId = Event.getMemberTeam(this.event, this.currentUserId ?? 0)?.id ?? 0;

            action = this.projectApiClient.createAsync(project);
          } else {
            action = this.projectApiClient.updateAsync(project);
          }

          action.subscribe({
            next:(_)=>{
              this.fetchProject()
            },
            error: errorContext => {
              this.errorProcessor.Process(errorContext)
            }
          });
        }
      });
  }

  public showUpdateProjectFromBitBranchDialog(){

    let dialogData = {
      eventId: this.project?.eventId,
      teamId: this.project?.teamId,
      linkToGitBranch: this.project?.linkToGitBranch
    } as IProjectUpdateFromGitBranch;

    this.dialogService.open(ProjectGitDialog, {
      data: dialogData
    })
      .afterClosed()
      .subscribe((data: IProjectUpdateFromGitBranch) => {
        this.projectApiClient
          .updateProjectFromGitBranch(data)
          .subscribe({
            next: (_) =>  {
              this.fetchProject()
            },
            error: (errorContext) => this.errorProcessor.Process(errorContext)
          });
      });
  }

  private fetchProject() {

    this.project = undefined;

    if (!this.currentUserId || !this.event) return;

    let team = Event.getMemberTeam(this.event, this.currentUserId);

    if (!team) return;

    this.projectApiClient.getAsync(this.event.id, team.id)
      .subscribe((r: IProject) => {
        this.project = r
      });
  }

  removeProject() {
    this.projectApiClient.remove(this.project?.eventId ?? 0, this.project?.teamId ?? 0)
      .subscribe(_ => {
        this.fetchProject()
      });
  }
}

enum Pages {
  EventTasks = 0,
  Communication = 1,
  Project = 2
}

enum ChatContext {
  Event = 0,
  Team = 1
}
