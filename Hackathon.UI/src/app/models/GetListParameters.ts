import {PageSettingsDefaults} from "./PageSettings";

export class GetListParameters<T>{
  Filter!: T;
  Offset:number = PageSettingsDefaults.Offset;
  Limit: number = PageSettingsDefaults.Limit;
  SortBy!: string;
  SortOrder: SortOrder = SortOrder.Asc;
}

export enum SortOrder {
  Asc = 0,
  Desc = 1
}

