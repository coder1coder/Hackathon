import {Component, Input} from '@angular/core';
import {AuthService} from "../../../services/auth.service";
import {MenuItem} from "../../../common/MenuItem";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent {

  @Input()
  items:MenuItem[] = []

  constructor(authService:AuthService) {
    authService.getCurrentUser()?.subscribe(x=>{
      this.items = this.items.filter(item=> !item.onlyForRoles || item.onlyForRoles.includes(x.role));
    })
  }
}
