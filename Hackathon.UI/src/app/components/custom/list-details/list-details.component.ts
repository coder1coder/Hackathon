import { Component, Input } from '@angular/core';
import { KeyValue } from '@angular/common';

@Component({
  selector: 'list-details',
  templateUrl: './list-details.component.html',
  styleUrls: ['./list-details.component.scss'],
})
export class ListDetailsComponent {
  @Input() items: KeyValue<string, any>[] = [];
}
