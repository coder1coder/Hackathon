import {Injectable, Input} from "@angular/core";
import {Event} from "../../../../models/Event/Event";

@Injectable()
export abstract class EventCardBaseComponent {
  @Input() isLoading: boolean = false;
  @Input() event: Event;

  protected eventId: number;

  public getDisplayTeamsColumns(): string[] {
    return ['name', 'members'];
  }
}
