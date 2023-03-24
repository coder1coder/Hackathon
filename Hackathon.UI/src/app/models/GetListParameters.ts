import {PageSettingsDefaults} from "./PageSettings";

export class GetListParameters<T> implements PaginationSorting
{
  Filter!: T;
  Limit: number;
  Offset: number;
  SortBy: string;
  SortOrder: SortOrder;
}

export class PaginationSorting
{
  Offset:number = PageSettingsDefaults.Offset;
  Limit: number = PageSettingsDefaults.Limit;
  SortBy!: string;
  SortOrder: SortOrder = SortOrder.Asc;
}

export enum SortOrder {
  Asc = 0,
  Desc = 1
}

