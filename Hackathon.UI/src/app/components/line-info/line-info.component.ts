import { Component, Input } from '@angular/core';

@Component({
  selector: 'line-info',
  template: `
    <div class="info-line grid">
      <div class="info-label grid-1-2">
        <span>{{label}}: </span>
      </div>
      <div class="info-value grid-1-4">{{value ? value : 'Не указано'}}</div>
    </div>
  `,
  styleUrls: ['./line-info.component.scss']
})
export class LineInfoComponent {
  @Input() label: string;
  @Input() value: string | number;
}
