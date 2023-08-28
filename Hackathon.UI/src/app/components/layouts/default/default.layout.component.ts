import {Component} from "@angular/core";
import {AppStateService} from "../../../services/state/app-state.service";

@Component({
  selector: 'layout-default',
  templateUrl: './default.layout.component.html',
  styleUrls: ['./default.layout.component.scss'],
})

export class DefaultLayoutComponent {

  public title: string | undefined;
  public hideTitleBar: boolean;
  public hideContentWhileLoading: boolean;
  public isLoading: boolean;
  public showLoadingIndicator: boolean;
  public containerCssClasses: string;
  public layoutCssClasses: string;
  public logoMinWidth: string;

  constructor(private appStateService: AppStateService) {

    this.appStateService.onTitleChanged.subscribe({next: (value) => this.title = value});
    this.appStateService.onHideTitleBarChanged.subscribe({next: (value) => this.hideTitleBar = value});
    this.appStateService.onHideContentWhileLoadingChanged.subscribe({next: (value) => this.hideContentWhileLoading = value});
    this.appStateService.onIsLoadingChanged.subscribe({next: (value) => this.isLoading = value});
    this.appStateService.onShowLoadingIndicatorChanged.subscribe({next: (value) => this.showLoadingIndicator = value});
    this.appStateService.onContainerCssClassesChanged.subscribe({next: (value) => this.containerCssClasses = value});
    this.appStateService.onLogoMinWidthChanged.subscribe({next: (value) => this.logoMinWidth = value});
    this.appStateService.onLayoutCssClassesChanged.subscribe({next: (value) => this.layoutCssClasses = value});

  }
}

