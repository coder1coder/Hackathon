import {Component, Input} from "@angular/core";
import {TeamModel} from "../../../models/Team/TeamModel";
import {RouterService} from "../../../services/router.service";

@Component({
  selector: 'team',
  templateUrl: './team.component.html',
  styleUrls: ['./team.component.scss']
})
export class TeamComponent {

  @Input() team?: TeamModel

  constructor(public router:RouterService) {
  }

}
