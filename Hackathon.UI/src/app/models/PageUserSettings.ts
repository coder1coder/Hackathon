import {PageUser} from "@angular/material/paginator";

export class PageUserSettings {

  page:number = 0;
  pageSize:number = PageSettingsDefaults.PageSize;
  sortBy:string | undefined;
  sortOrder:SortOrder = SortOrder.ASC;

  constructor(pageUser:PageUser) {
    this.page = pageUser.pageIndex;
    this.pageSize = pageUser.pageSize;
  }

  toQueryArgs(){
    return `&Page=${this.page+1}&PageSize=${this.pageSize}&SortBy=${this.sortBy}&SortOrder=${this.sortOrder}`;
  }

}

export enum PageUserSettingsDefaults {
  PageSize = 10,
  PageIndex = 0
}

export enum SortOrder {
  ASC = 0,
  DESC = 1
}
