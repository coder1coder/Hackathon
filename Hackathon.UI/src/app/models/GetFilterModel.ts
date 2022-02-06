export class GetFilterModel<T>{
  Filter!: T;
  PageSize: number = 300;
  SortBy!: string;
  SortOrder: SortOrder = SortOrder.Asc;


    private _page: number = 1;

    public get Page(): number {
        return this._page;
    }

    public set Page(n: number) {
      this._page = n + 1;
    }
}

export enum SortOrder {
  Asc = 0,
  Desc = 1
}

