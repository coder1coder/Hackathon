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
        new MenuItem('/users','Пользователи', x.role == UserRole.Administrator),
        new MenuItem('/team','Команда'),
      ].filter(x=>x.access);
    })
  }
}

class MenuItem {
  routeLink:string
  text:string
  access:boolean

  constructor(routeLink:string, text:string, access:boolean = true) {
    this.routeLink = routeLink;
    this.text = text;
    this.access = access;
  }
}
