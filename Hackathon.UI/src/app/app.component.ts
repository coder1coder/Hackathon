import { Component, OnInit } from '@angular/core';
import { ThemeChangeService } from "./services/theme-change.service";
import { SignalRService } from "./services/signalr.service";
import { environment } from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public title = 'Hackathon.UI';

  constructor(
    private signalRService: SignalRService,
    private themeChangeService: ThemeChangeService,
  ) {}

  ngOnInit(): void {
    this.signalRService.initSignalR(environment.hubs.chat);
    this.signalRService.initSignalR(environment.hubs.notification);
    this.signalRService.initSignalR(environment.hubs.friendship);
    this.signalRService.initSignalR(environment.hubs.event);
    this.themeChangeService.initThemeMode();
  }
}
