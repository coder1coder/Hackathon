import {AfterViewInit, Component, OnInit} from '@angular/core';
import {ThemeChangeService} from "./services/theme-change.service";
import {AppStateService} from "./services/state/app-state.service";
import { Router,NavigationEnd  } from '@angular/router';
import {filter} from "rxjs";
import {LoginComponent} from "./components/login/login.component";

@Component({
  selector: 'app-root',
  template: `
    <ng-container *ngIf="this.useLayout; else withoutLayout">
      <layout-default>
        <router-outlet></router-outlet>
      </layout-default>
    </ng-container>
    <ng-template #withoutLayout><router-outlet></router-outlet></ng-template>

  `,
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit, OnInit {
  title = 'Hackathon.UI';
  useLayout = true;

  constructor(
    private themeChangeService: ThemeChangeService,
    public appStateService: AppStateService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.router.events.
    pipe(filter(x=>x instanceof NavigationEnd))
      .subscribe({
        next: (routerEvent) =>  {
          if (routerEvent instanceof NavigationEnd){
            let route = this.router.config.find(x=> `/${x.path}` == routerEvent.url);
            let componentName = route?.component?.name ?? '';
            this.useLayout = !['LoginComponent'].includes(componentName);
          }
        }});
    }

  ngAfterViewInit() {
    this.themeChangeService.initThemeMode();


  }
}
