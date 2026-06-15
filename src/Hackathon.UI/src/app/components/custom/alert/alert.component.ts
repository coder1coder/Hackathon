import { Component, HostBinding, Input, HostAttributeToken, inject } from '@angular/core';

import { MatIcon } from '@angular/material/icon';

@Component({
    selector: 'alert',
    templateUrl: './alert.component.html',
    styleUrls: ['./alert.component.scss'],
    imports: [MatIcon]
})
export class AlertComponent {
  isWarn? = inject(new HostAttributeToken('warn'), { optional: true });
  isDanger? = inject(new HostAttributeToken('danger'), { optional: true });

  @Input() showIcon: boolean = true;
  @Input() closeable: boolean = false;
  @Input() icon = 'info_outline';

  @HostBinding(`class.closed`) isClosed = false;

  constructor() {
    if (this.isWarn !== null) this.icon = 'warning_amber';

    if (this.isDanger !== null) this.icon = 'error_outline';
  }
}
