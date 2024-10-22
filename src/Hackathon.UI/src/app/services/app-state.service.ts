import { action, makeObservable, observable, runInAction } from 'mobx';
import { Injectable } from '@angular/core';
import * as _ from 'lodash';

@Injectable({
  providedIn: 'root',
})
export class AppStateService {
  @observable isLoading: boolean = false;

  constructor() {
    makeObservable(this);
  }

  /**
   * Установка стэйта isLoading с зарержкой можно использовать в глобальном перехвадчике запросов.
   * Например, для того чтобы, дождаться пока последний запрос на старнице выполнится и установить
   * флаг как = false. Подписавшись на свойство isLoading можно управлять бубликом глобально к примеру
   * для загрузки всей страницы.
   * @param state Булевое состояние загрузки запроса
   */
  @action
  public setIsLoadingStateWithDebounce: _.DebouncedFunc<(state: boolean) => void> = _.debounce(
    this.setIsLoadingState,
    50,
  );

  /**
   * Установить состояние свойства из isLoading
   * @param state Булевое значение состояния загрузки запроса
   */
  @action
  public setIsLoadingState(state: boolean): void {
    runInAction(() => {
      this.isLoading = state;
    });
  }
}
