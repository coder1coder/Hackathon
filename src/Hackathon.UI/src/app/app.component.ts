import { Component, OnInit, inject } from '@angular/core';
import { ThemeChangeService } from './services/theme-change.service';
import { SignalRService } from './services/signalr.service';
import { environment } from '../environments/environment';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [RouterOutlet],
})
export class AppComponent implements OnInit {
  private signalRService = inject(SignalRService);
  private themeChangeService = inject(ThemeChangeService);

  public title: string = 'Hackathon.UI';



  ngOnInit(): void {
    this.signalRService.initSignalR(environment.hubs.chats.events);
    this.signalRService.initSignalR(environment.hubs.chats.teams);
    this.signalRService.initSignalR(environment.hubs.notification);
    this.signalRService.initSignalR(environment.hubs.friendship);
    this.signalRService.initSignalR(environment.hubs.event);
    this.themeChangeService.initThemeMode();
  }
}
