import { Event } from "../../models/Event/Event";
import { EventStatus } from "../../models/Event/EventStatus";
import { AuthService } from "../auth.service";
import { EventEmitter, Injectable } from "@angular/core";
import { ApprovalApplicationStatusEnum } from "../../models/approval-application/approval-application-status.enum";
import { isNull, isUndefined } from "lodash";
import { catchError, filter, Observable, of } from "rxjs";
import { map } from "rxjs/operators";
import { RouterService } from "../router.service";
import { EventErrorMessages } from "../../common/error-messages/event-error-messages";
import { SnackService } from "../snack.service";
import { GlobalErrorHandler } from "../../common/handlers/error.handler";
import { EventClient } from "./event.client";
import moment from "moment";

@Injectable({
  providedIn: 'root'
})

export class EventService {
  public reloadEvent = new EventEmitter<boolean>();

  constructor(
    private authService: AuthService,
    private routerService: RouterService,
    private snackService: SnackService,
    private globalErrorHandler: GlobalErrorHandler,
    private eventClient: EventClient,
  ) {
  }

  public checkAccessViewEventById(eventId: number): Observable<boolean> {
    if (!this.authService.isLoggedIn()) {
      this.routerService.Profile.Login();
      return of(false);
    }

    if (isNull(eventId) || isUndefined(eventId)) {
      this.routerService.Events.List().then(() =>
        this.snackService.open(EventErrorMessages.EventNotFound));
      return of(false);
    }

    return this.eventClient.getById(eventId)
      .pipe(
        filter((event: Event) => !!event),
        map((event: Event) => this.canView(event)),
        catchError((err) => {
          this.globalErrorHandler.handleError(err);
          return of(false);
        }),
      );
  }

  public checkAccessViewEventByModel(event: Event): Observable<boolean> {
    if (!this.authService.isLoggedIn()) {
      this.routerService.Profile.Login();
      return of(false);
    }

    if (isNull(event) || isUndefined(event)) {
      this.routerService.Events.List().then(() =>
        this.snackService.open(EventErrorMessages.EventNotFound));
      return of(false);
    }

    return of(this.canView(event));
  }

  public isCanJoinToEvent(event: Event): boolean {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && !this.isAlreadyInEvent(event, userId)
      && event.status === EventStatus.Published;
  }

  public isCanFinishEvent(event: Event): boolean {
    return this.isEventOwner(event)
      && event.status !== EventStatus.Finished
      && event.status !== EventStatus.Draft;
  }

  public isCanStartEvent(event: Event): boolean {
    const totalMembers = Event.getUsersCount(event);
    const minimalMembers = event.minTeamMembers * 2;
    return this.isEventOwner(event)
      && event.status === EventStatus.Published
      && totalMembers >= minimalMembers;
  }

  public isCanLeave(event: Event): boolean {
    let userId = this.authService.getUserId();
    return event.status !== EventStatus.Finished
      && userId !== undefined
      && this.isAlreadyInEvent(event, userId);
  }

  public isCanAddTeam(event: Event): boolean {
    return this.isEventOwner(event)
      && event.status === EventStatus.Published
      && !event.isCreateTeamsAutomatically;
  }

  public isAlreadyInEvent(event: Event, userId: number): boolean {
    return event.teams?.filter(t => t
      .members?.filter(x => x.id === userId)
      .length > 0
    ).length > 0;
  }

  public isEventOwner(event: Event): boolean {
    return event?.owner?.id === this.authService.getUserId();
  }

  public isCanPublish(event: Event): boolean {
    return event?.id !== undefined
      && !isNaN(Number(event?.id))
      && Boolean(event?.approvalApplicationId)
      && this.isEventOwner(event)
      && event?.status === EventStatus.OnModeration
      && event?.approvalApplication?.applicationStatus === ApprovalApplicationStatusEnum.Approved
      && moment(event?.start).isSameOrAfter(new Date());
  }

  public isCanOnModeration(event: Event): boolean {
    return Boolean(event?.id)
      && this.isEventOwner(event)
      && !event.approvalApplicationId;
  }

  public isCanDeleteEvent(event: Event): boolean {
    const userId = this.authService.getUserId();
    return event?.id !== undefined
      && userId !== null
      && event?.owner?.id === userId;
  }

  public isCreateEditEvent(event: Event): boolean {
    return event?.id === undefined ||
      event?.status === EventStatus.Draft ||
      event?.status === EventStatus.OnModeration;
  }

  public canView(event: Event): boolean {
    const userId = this.authService.getUserId();
    if (isNull(userId) || isUndefined(userId) || isNull(event) || isUndefined(event)) {
      return false;
    }

    switch (event?.status) {
      case EventStatus.Started:
        return this.isAlreadyInEvent(event, userId) || this.isEventOwner(event);
    }

    return true;
  }
}
