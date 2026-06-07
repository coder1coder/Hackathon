import { booleanAttribute, Component, Input } from '@angular/core';
import { ToolbarComponent } from '../../toolbar/toolbar.component';
import { NgIf } from '@angular/common';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

@Component({
    selector: 'layout-default',
    templateUrl: './default.layout.component.html',
    styleUrls: ['./default.layout.component.scss'],
    imports: [
        ToolbarComponent,
        NgIf,
        MatProgressSpinner,
    ],
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
