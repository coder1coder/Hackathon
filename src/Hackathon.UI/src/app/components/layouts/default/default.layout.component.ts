import { booleanAttribute, Component, Input } from '@angular/core';

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
  standalone: false,
})
export class DefaultLayoutComponent {
  @Input() title: string = '';
  @Input() subTitle: string | null = '';
  @Input() hideTitleBar: boolean = false;
  @Input() hideContentWhileLoading: boolean = true;
  @Input({ transform: booleanAttribute }) isLoading: boolean = false;
  @Input() showLoadingIndicator: boolean = true;
  @Input() containerCssClasses: string = 'container container-full container-padding';
  @Input() layoutCssClasses: string = '';
  @Input() logoMinWidth: string = `initial`;
}
