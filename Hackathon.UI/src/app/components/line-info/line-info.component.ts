import { ChangeDetectionStrategy, Component, Input, TemplateRef } from '@angular/core';

@Component({
  selector: 'app-line-info',
  templateUrl: './line-info.component.html',
  styleUrls: ['./line-info.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LineInfoComponent {
  @Input() label: string;
  @Input() value: string | number;
  @Input() clickable: boolean = false;
  @Input() customTemplate: TemplateRef<any>;
}
