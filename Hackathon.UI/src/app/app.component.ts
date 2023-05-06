import {Component, OnInit} from '@angular/core';
import {ThemeChangeService} from "./services/theme-change.service";
import {fromMobx} from "./common/functions/from-mobx.function";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Hackathon.UI';

  constructor(
    private themeChangeService: ThemeChangeService
  ) {
  }

  ngOnInit() {
    this.initThemeMode();
  }

  private initThemeMode(): void {
    fromMobx(() => this.themeChangeService.themeMode)
    .subscribe((theme) => {
      const mode = this.themeChangeService.getMode();
      this.themeChangeService.setThemeMode(mode ?? theme);
    })
  }
}
