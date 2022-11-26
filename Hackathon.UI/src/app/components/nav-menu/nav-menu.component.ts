import { Component } from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {UserRole} from "../../models/User/UserRole";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  items:MenuItem[] = []

  constructor(authService:AuthService) {

    authService.getCurrentUser()?.subscribe(x=>{
      this.items = [
        new MenuItem('/events','События'),
        new MenuItem('/team','Моя команда'),
        new MenuItem('#', 'Администрирование', x.role == UserRole.Administrator,[
          new MenuItem('/users','Пользователи'),
          new MenuItem('/eventLog','Журнал событий'),
        ]),
      ].filter(x=>x.access);
    })
  }
}

class MenuItem {
  unique: number
  routeLink:string
  text:string
  access:boolean
  children: MenuItem[]

  constructor(routeLink:string, text:string, access:boolean = true, children: MenuItem[] = []) {
    this.unique = new Date().getTime();

    this.routeLink = routeLink;
    this.text = text;
    this.access = access;
    this.children = children;
  }
}
