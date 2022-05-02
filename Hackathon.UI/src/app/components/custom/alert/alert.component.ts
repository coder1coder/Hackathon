import {Attribute, Component, HostBinding, Input} from "@angular/core";

@Component({
  selector: 'alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.scss']
})

export class AlertComponent {

  @Input()
  public showIcon:boolean = true;

  @Input()
  public closeable:boolean = false;

  @HostBinding(`class.closed`) isClosed = false;

  public icon = 'info_outline';

  constructor(
    @Attribute('warn') public isWarn?: boolean,
    @Attribute('danger') public isDanger?: boolean) {

    if (this.isWarn != null)
      this.icon = 'warning_amber';

    if (this.isDanger != null)
      this.icon = 'error_outline';

  }
}
