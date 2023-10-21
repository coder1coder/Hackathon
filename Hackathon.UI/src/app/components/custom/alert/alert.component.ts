import { Attribute, Component, HostBinding, Input } from "@angular/core";

@Component({
  selector: 'alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss'],
})

export class AlertComponent {

  @Input() showIcon: boolean = true;
  @Input() closeable: boolean = false;
  @Input() icon = 'info_outline';

  @HostBinding(`class.closed`) isClosed = false;

  constructor(
    @Attribute('warn') public isWarn?: boolean,
    @Attribute('danger') public isDanger?: boolean) {

    if (this.isWarn !== null)
      this.icon = 'warning_amber';

    if (this.isDanger !== null)
      this.icon = 'error_outline';

  }
}
