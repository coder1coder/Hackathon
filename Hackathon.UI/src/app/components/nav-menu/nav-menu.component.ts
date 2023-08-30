import {Component, Input} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {MenuItem} from "../../common/MenuItem";
import { UserRole } from 'src/app/models/User/UserRole';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  items: MenuItem[] =[]

  constructor(authService:AuthService) {
    authService.getCurrentUser()?.subscribe(x=>{
      this.items = [
        new MenuItem('/events','События'),
        new MenuItem('/team','Моя команда'),
        new MenuItem('#', 'Администрирование', [UserRole.Administrator],[
          new MenuItem('/users','Пользователи'),
          new MenuItem('/eventLog','Журнал событий'),
        ])
      ].filter(item=> !item.onlyForRoles || item.onlyForRoles.includes(x.role));
    })
  }
}
