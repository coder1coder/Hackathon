import { Injectable, DOCUMENT, inject } from '@angular/core';
import { action, makeObservable, observable, runInAction } from 'mobx';
import { AuthConstants } from './auth.constants';
import { IThemeModeInterface } from '../common/interfaces/theme-mode.interface';

import { OverlayContainer } from '@angular/cdk/overlay';
import { fromMobx } from '../common/functions/from-mobx.function';
import { filter } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ThemeChangeService {
  private document = inject<Document>(DOCUMENT);
  private overlay = inject(OverlayContainer);

  private storage: Storage = sessionStorage;
  private darkClassName = 'theme-dark-mode';
  private lightClassName = 'theme-light-mode';
  @observable themeMode: IThemeModeInterface = this.getMode();

  constructor() {
    makeObservable(this);
  }

  public initThemeMode(): void {
    fromMobx(() => this.themeMode)
      .pipe(filter(Boolean))
      .subscribe((theme) => {
        const mode: IThemeModeInterface = this.getMode();
        this.setThemeMode(mode ?? theme);
      });
  }

  @action
  public changeMode(): void {
    runInAction(() => {
      const currentMode: boolean = !this.themeMode?.isDarkMode;
      this.themeMode = {
        modeClass: currentMode ? this.darkClassName : this.lightClassName,
        isDarkMode: currentMode,
      };
      this.storage.setItem(AuthConstants.STORAGE_MODE_KEY, JSON.stringify(this.themeMode));
    });
  }

  private getMode(): IThemeModeInterface {
    const mode: string | null = this.storage.getItem(AuthConstants.STORAGE_MODE_KEY);
    return mode ? JSON.parse(mode) : { modeClass: this.lightClassName, isDarkMode: false };
  }

  private setThemeMode(theme: IThemeModeInterface): void {
    if (theme?.isDarkMode) {
      this.overlay.getContainerElement().classList.add(theme.modeClass);
      this.overlay.getContainerElement().classList.remove(this.lightClassName);
      if (this.document.firstElementChild !== null)
        this.document.firstElementChild.classList.add(theme.modeClass);
      if (this.document.firstElementChild !== null)
        this.document.firstElementChild.classList.remove(this.lightClassName);
    } else {
      this.overlay.getContainerElement().classList.add(theme.modeClass);
      this.overlay.getContainerElement().classList.remove(this.darkClassName);
      if (this.document.firstElementChild !== null)
        this.document.firstElementChild.classList.add(theme.modeClass);
      if (this.document.firstElementChild !== null)
        this.document.firstElementChild.classList.remove(this.darkClassName);
    }
  }
}
