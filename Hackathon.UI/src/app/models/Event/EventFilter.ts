import {EventStatus} from "./EventStatus";

export class EventFilter {
  ids?: number[];
  name?: string;
  startFrom?: Date;
  startTo?: Date;
  statuses?: EventStatus[];
  excludeOtherUsersDraftedEvents?: boolean;
  teamsIds?: number[];
}
