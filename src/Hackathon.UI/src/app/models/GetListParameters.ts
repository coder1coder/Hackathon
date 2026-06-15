import { PageSettingsDefaults } from './PageSettings';

export class GetListParameters<T> implements PaginationSorting {
  Filter?: T;
  Limit: number = 0;
  Offset: number = 0;
  SortBy: string = '';
  SortOrder: SortOrder = 0;
}

export class PaginationSorting {
  Offset: number = PageSettingsDefaults.Offset;
  Limit: number = PageSettingsDefaults.Limit;
  SortBy: string = '';
  SortOrder: SortOrder = SortOrder.Asc;
}

export enum SortOrder {
  Asc = 0,
  Desc = 1,
}
