import {Event} from "../../models/Event/Event";
import {EventStatus} from "../../models/Event/EventStatus";
import {AuthService} from "../auth.service";
import {EventEmitter, Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class EventService {
  public reloadEvent = new EventEmitter<boolean>();

  constructor(
    private authService: AuthService
  ) {
  }

  public isCanJoinToEvent(event: Event): boolean {
    let userId = this.authService.getUserId();
    return userId !== undefined
      && !this.isAlreadyInEvent(event, userId)
      && event.status === EventStatus.Published
  }

  public isCanFinishEvent(event: Event): boolean {
    return this.isEventOwner(event)
      && event.status !== EventStatus.Finished
      && event.status !== EventStatus.Draft
  }

  public isCanPublishEvent(event: Event): boolean {
    return this.isEventOwner(event)
      && event.status === EventStatus.Draft;
  }

  public isCanStartEvent(event: Event): boolean {
    const totalMembers = Event.getUsersCount(event);
    const minimalMembers = event.minTeamMembers * 2;
    return this.isEventOwner(event)
      && event.status === EventStatus.Published
      && totalMembers <= minimalMembers;
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
      && !event.isCreateTeamsAutomatically
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
    return event?.id !== undefined && !isNaN(Number(event?.id))
      && this.isCanPublishEvent(event);
  }

  public isCanDeleteEvent(event: Event): boolean {
    const userId = this.authService.getUserId();
    return event?.id !== undefined
      && userId !== null
      && event?.owner?.id === userId;
  }

  public isCreateEditEvent(event: Event): boolean {
    return event?.id === undefined || event?.status === EventStatus.Draft;
  }

  public canView(event: Event): boolean {
    const userId = this.authService.getUserId();
    if (userId === undefined)
    {
      return false;
    }

    switch (event.status)
    {
      case EventStatus.Started:
        return this.isAlreadyInEvent(event, userId) || this.isEventOwner(event);
    }

    return true;
  }
}
