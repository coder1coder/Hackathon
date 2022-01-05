import {Component, Input} from "@angular/core";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  @Input() title!: string;

  constructor() {
  }
}
