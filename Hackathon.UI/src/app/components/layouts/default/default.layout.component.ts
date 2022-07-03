import {Component, Input} from "@angular/core";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  @Input() title!: string;
  @Input() hideTitlebar: boolean = false;
  @Input() isLoading: boolean = false;
  @Input() containerClasses: string = 'container container-full';

  constructor() {}
}

