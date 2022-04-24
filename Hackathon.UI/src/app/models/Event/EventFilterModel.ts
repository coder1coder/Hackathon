import {EventStatus} from "../EventStatus";

export class EventFilterModel {
  ids?: number[];
  name?: string;
  startFrom?: Date;
  startTo?: Date;
  statuses?: EventStatus[];
  excludeOtherUsersDraftedEvents?: boolean;
}
