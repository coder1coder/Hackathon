export class GetFilterModel<T>{
  Filter!: T;
  Page:number = 0;
  PageSize: number = 300;
  SortBy!: string;
  SortOrder: SortOrder = SortOrder.Asc;
}

export enum SortOrder {
  Asc = 0,
  Desc = 1
}

