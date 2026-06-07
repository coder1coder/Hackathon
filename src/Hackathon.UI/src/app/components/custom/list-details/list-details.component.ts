import { Component, Input } from '@angular/core';
import { KeyValue, NgIf, NgFor } from '@angular/common';

@Component({
    selector: 'list-details',
    templateUrl: './list-details.component.html',
    styleUrls: ['./list-details.component.scss'],
    imports: [NgIf, NgFor]
})
export class ListDetailsComponent {
  @Input() items: KeyValue<string, any>[] = [];
}
