import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'line-info',
  templateUrl: './line-info.component.html',
  styleUrls: ['./line-info.component.scss']
})
export class LineInfoComponent implements OnInit {

  @Input() label: string;
  @Input() value: string | number;

  constructor() { }

  ngOnInit(): void {
  }

}
