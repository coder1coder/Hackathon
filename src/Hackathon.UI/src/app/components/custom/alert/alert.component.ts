import { Attribute, Component, HostBinding, Input } from '@angular/core';
import { NgIf } from '@angular/common';
import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'alert',
    templateUrl: './alert.component.html',
    styleUrls: ['./alert.component.scss'],
    imports: [NgIf, MatIcon]
})
export class AlertComponent {
  @Input() showIcon: boolean = true;
  @Input() closeable: boolean = false;
  @Input() icon = 'info_outline';

  @HostBinding(`class.closed`) isClosed = false;

  constructor(
    @Attribute('warn') public isWarn?: boolean,
    @Attribute('danger') public isDanger?: boolean,
  ) {
    if (this.isWarn !== null) this.icon = 'warning_amber';

    if (this.isDanger !== null) this.icon = 'error_outline';
  }
}
