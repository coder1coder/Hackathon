import {Injectable} from "@angular/core";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: 'root'
})

export class AppStateService
{
  set title(value) { this.onTitleChanged.next(value); };
  get title() { return this.onTitleChanged.getValue(); }
  public onTitleChanged = new BehaviorSubject<string | undefined>(undefined);

  set hideTitleBar(value) { this.onHideTitleBarChanged.next(value); };
  get hideTitleBar() { return this.onHideTitleBarChanged.getValue(); }
  public onHideTitleBarChanged = new BehaviorSubject<boolean>(false);

  set hideContentWhileLoading(value) { this.onHideContentWhileLoadingChanged.next(value); };
  get hideContentWhileLoading() { return this.onHideContentWhileLoadingChanged.getValue(); }
  public onHideContentWhileLoadingChanged = new BehaviorSubject<boolean>(true);

  set isLoading(value) { this.onIsLoadingChanged.next(value); };
  get isLoading() { return this.onIsLoadingChanged.getValue(); }
  public onIsLoadingChanged = new BehaviorSubject<boolean>(false);

  set showLoadingIndicator(value) { this.onShowLoadingIndicatorChanged.next(value); };
  get showLoadingIndicator() { return this.onShowLoadingIndicatorChanged.getValue(); }
  public onShowLoadingIndicatorChanged = new BehaviorSubject<boolean>(true);

  set containerCssClasses(value) { this.onContainerCssClassesChanged.next(value); };
  get containerCssClasses() { return this.onContainerCssClassesChanged.getValue(); }
  public onContainerCssClassesChanged = new BehaviorSubject<string>('container container-full container-padding');

  set logoMinWidth(value) { this.onLogoMinWidthChanged.next(value); };
  get logoMinWidth() { return this.onLogoMinWidthChanged.getValue(); }
  public onLogoMinWidthChanged = new BehaviorSubject<string>('initial');

  set layoutCssClasses(value) { this.onLayoutCssClassesChanged.next(value); };
  get layoutCssClasses() { return this.onLayoutCssClassesChanged.getValue(); }
  public onLayoutCssClassesChanged = new BehaviorSubject<string>('');
}
