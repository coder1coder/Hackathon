import {PageEvent} from "@angular/material/paginator";

export class PageSettings {

  offset: number = PageSettingsDefaults.Offset;
  limit: number = PageSettingsDefaults.Limit;
  sortBy: string;
  sortOrder: SortOrder = SortOrder.ASC;

  constructor(pageEvent:PageEvent) {
    this.offset = pageEvent.pageIndex;
    this.limit = pageEvent.pageSize;
  }
}

export enum PageSettingsDefaults {
  Limit = 15,
  Offset = 0
}

export enum SortOrder {
  ASC = 0,
  DESC = 1
}
