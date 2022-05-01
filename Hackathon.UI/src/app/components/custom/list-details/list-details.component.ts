import {Component, Input} from "@angular/core";
import {KeyValuePair} from "../../../common/KeyValuePair";

@Component({
  selector: 'list-details',
  templateUrl: './list-details.component.html',
  styleUrls: ['./list-details.component.scss']
})

export class ListDetailsComponent
{
  @Input()
  public items: KeyValuePair[] = [];

  constructor() {
  }
}
