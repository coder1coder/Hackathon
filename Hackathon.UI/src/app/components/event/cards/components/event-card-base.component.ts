import { Injectable, Input, OnDestroy } from "@angular/core";
import { Event } from "../../../../models/Event/Event";
import { Observable, Subject } from "rxjs";
import { fromMobx } from "../../../../common/functions/from-mobx.function";
import { AppStateService } from "../../../../services/app-state.service";

@Injectable()
export abstract class EventCardBaseComponent implements OnDestroy {

  @Input() event: Event;
  public isLoading$: Observable<boolean> = fromMobx(() => this.appStateService.isLoading);

  protected eventId: number;
  protected destroy$ = new Subject();

  protected constructor(protected appStateService: AppStateService) {}

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public getDisplayTeamsColumns(): string[] {
    return ['name', 'members'];
  }
}
