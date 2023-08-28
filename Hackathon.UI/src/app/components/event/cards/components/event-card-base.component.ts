import {Injectable, Input, OnDestroy} from "@angular/core";
import {Event} from "../../../../models/Event/Event";
import {Subject} from "rxjs";

@Injectable()
export abstract class EventCardBaseComponent implements OnDestroy {
  @Input() event: Event;

  protected eventId: number;

  protected destroy$ = new Subject();

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  public getDisplayTeamsColumns(): string[] {
    return ['name', 'members'];
  }
}
