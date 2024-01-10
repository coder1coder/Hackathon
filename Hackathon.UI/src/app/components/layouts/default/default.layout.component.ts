import { Component, Input } from '@angular/core';

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})
export class DefaultLayoutComponent {
  @Input() title: string;
  @Input() subTitle: string;
  @Input() hideTitleBar: boolean = false;
  @Input() hideContentWhileLoading: boolean = true;
  @Input() isLoading: boolean = false;
  @Input() showLoadingIndicator: boolean = true;
  @Input() containerCssClasses: string = 'container container-full container-padding';
  @Input() layoutCssClasses: string = '';
  @Input() logoMinWidth: string = `initial`;
}
