import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material/paginator';

@Injectable()
export class Pagination extends MatPaginatorIntl {
  public override itemsPerPageLabel = 'На странице';

  public override getRangeLabel = function (
    page: number,
    pageSize: number,
    length: number,
  ): string {
    if (length === 0 || pageSize === 0) {
      return '0 из ' + length;
    }
    length = Math.max(length, 0);
    const startIndex: number = page * pageSize;
    const endIndex: number =
      startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
    return startIndex + 1 + ' - ' + endIndex + ' из ' + length;
  };
}
