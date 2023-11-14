import { Inject, Injectable } from "@angular/core";
import { action, makeObservable, observable, runInAction } from "mobx";
import { AuthConstants } from "./auth.constants";
import { IThemeModeInterface } from "../common/theme-mode.interface";
import { DOCUMENT } from "@angular/common";
import { OverlayContainer } from "@angular/cdk/overlay";
import { fromMobx } from "../common/functions/from-mobx.function";
import { filter } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ThemeChangeService {

  private storage: Storage = sessionStorage;
  private darkClassName = 'theme-dark-mode';
  private lightClassName = 'theme-light-mode';
  @observable themeMode: IThemeModeInterface = this.getMode();

  constructor(
    @Inject(DOCUMENT) private document: Document,
    private overlay: OverlayContainer,
  ) {
    makeObservable(this);
  }

  public initThemeMode(): void {
    fromMobx(() => this.themeMode)
      .pipe(filter(Boolean))
      .subscribe((theme) => {
        const mode = this.getMode();
        this.setThemeMode(mode ?? theme);
      })
  }

  @action
  public changeMode(): void {
    runInAction(() => {
      const currentMode: boolean = !Boolean(this.themeMode?.isDarkMode);
      this.themeMode = {
        modeClass: currentMode ? this.darkClassName : this.lightClassName,
        isDarkMode: currentMode,
      };
      this.storage.setItem(AuthConstants.STORAGE_MODE_KEY, JSON.stringify(this.themeMode));
    })
  }

  private getMode(): IThemeModeInterface {
    const mode = this.storage.getItem(AuthConstants.STORAGE_MODE_KEY);
    return mode !== undefined ? JSON.parse(mode) : { modeClass: this.lightClassName, isDarkMode: false };
  }

  private setThemeMode(theme: IThemeModeInterface): void {
    if (theme?.isDarkMode) {
      this.overlay.getContainerElement().classList.add(theme.modeClass);
      this.overlay.getContainerElement().classList.remove(this.lightClassName);
      if (this.document.firstElementChild !== null) this.document.firstElementChild.classList.add(theme.modeClass);
      if (this.document.firstElementChild !== null) this.document.firstElementChild.classList.remove(this.lightClassName);
    } else {
      this.overlay.getContainerElement().classList.add(theme.modeClass);
      this.overlay.getContainerElement().classList.remove(this.darkClassName);
      if (this.document.firstElementChild !== null) this.document.firstElementChild.classList.add(theme.modeClass);
      if (this.document.firstElementChild !== null) this.document.firstElementChild.classList.remove(this.darkClassName);
    }
  }
}
