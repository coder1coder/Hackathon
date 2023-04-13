import {Component, Input} from "@angular/core";
import {MenuItem} from "../../../common/MenuItem";
import {UserRole} from "../../../models/User/UserRole";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  @Input() title: string;
  @Input() hideTitleBar: boolean = false;
  @Input() hideContentWhileLoading: boolean = true;
  @Input() isLoading: boolean = false;
  @Input() showLoadingIndicator: boolean = true;
  @Input() containerCssClasses: string = 'container container-full container-padding';
  @Input() layoutCssClasses: string = '';
  @Input() logoMinWidth: string = `initial`;

  toolbarMenuItems = [
    new MenuItem('/events','События'),
    new MenuItem('/team','Моя команда'),
    new MenuItem('#', 'Администрирование', [UserRole.Administrator],[
      new MenuItem('/users','Пользователи'),
      new MenuItem('/eventLog','Журнал событий'),
    ]),
  ]

  constructor() {
  }
}

