import { Component, OnInit } from '@angular/core';
import { ThemeChangeService } from "./services/theme-change.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public title = 'Hackathon.UI';

  constructor(private themeChangeService: ThemeChangeService) {}

  ngOnInit(): void {
    this.themeChangeService.initThemeMode();
  }
}
