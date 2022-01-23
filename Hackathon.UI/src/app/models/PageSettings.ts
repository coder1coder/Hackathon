import {PageEvent} from "@angular/material/paginator";

export class PageSettings {

  page:number = 0;
  pageSize:number = PageSettingsDefaults.PageSize;
  sortBy!:string;
  sortOrder:SortOrder = SortOrder.ASC;

  constructor(pageEvent:PageEvent) {
    this.page = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
  }

  toQueryArgs(){
    return `&Page=${this.page+1}&PageSize=${this.pageSize}&SortBy=${this.sortBy ?? ''}&SortOrder=${this.sortOrder}`;
  }
}

export enum PageSettingsDefaults {
  PageSize = 10,
  PageIndex = 0
}

export enum SortOrder {
  ASC = 0,
  DESC = 1
}
