import {Component} from "@angular/core";
import {AppStateService} from "../../../services/state/app-state.service";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  public title: string;
  public hideTitleBar: boolean;
  public hideContentWhileLoading: boolean;
  public isLoading: boolean;
  public showLoadingIndicator: boolean;
  public containerCssClasses: string;
  public layoutCssClasses: string;
  public logoMinWidth: string;

  constructor(private appStateService: AppStateService) {

    this.title = appStateService.title;
    this.hideTitleBar = appStateService.hideTitleBar;
    this.hideContentWhileLoading = appStateService.hideContentWhileLoading;
    this.isLoading = appStateService.isLoading;
    this.showLoadingIndicator = appStateService.showLoadingIndicator;
    this.containerCssClasses = appStateService.containerCssClasses;
    this.layoutCssClasses = appStateService.layoutCssClasses;
    this.logoMinWidth = appStateService.logoMinWidth;

  }
}

