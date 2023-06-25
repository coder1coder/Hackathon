import {PageEvent} from "@angular/material/paginator";
import {AfterViewInit, Directive, OnDestroy} from "@angular/core";
import {PageSettingsDefaults} from "../models/PageSettings";
import {GetListParameters} from "../models/GetListParameters";
import {Subject} from "rxjs";

@Directive()
export abstract class PageResultComponent<T> implements AfterViewInit, OnDestroy {

  public items: T[] = [];
  public pageSettings: PageEvent = new PageEvent();

  private readonly componentName: string | undefined;

  /** Отказ от подписки вручную в связке с takeUntil
   * takeUntil принимает на вход объект Observable как параметр notifier
   * когда notifier выпускает значение, выполняет отписку от исходного Observable.
   * оповещает Observable об уничтожении компонента.
   * Для этого мы добавляем свойство класса с именем componentDestroyed$ (или любым другим именем)
   * типа Subject<void> и используем его в качестве notifier.
   * Нам нужно только добавить takeUntil(componentDestroyed$), а RxJS позаботится об остальном
   * Отказ от ненужных подписок предотвращает утечку памяти. Кроме того, декларативная отписка не требует ссылки на подписки.
   */
  protected destroy$ = new Subject();

  protected constructor(
    private name:string
  ) {
    this.componentName = name;
    let pageSettingsJson = sessionStorage.getItem(`${this.componentName}${PageEvent.name}`);

    if (pageSettingsJson != null)
      this.pageSettings = JSON.parse(pageSettingsJson)
    else {
      this.pageSettings.pageSize = PageSettingsDefaults.Limit;
      this.pageSettings.pageIndex = 0;
    }
  }

  public abstract rowClick(item: T):any;
  public abstract fetch(getFilterModel?: GetListParameters<T>):any;

  ngAfterViewInit(): void {
    this.fetch();
  }

  public paginatorChanges(event:PageEvent): void {
    this.setPageSettings(event);
    this.fetch();
  }

  public setPageSettings(event:PageEvent): void {
    this.pageSettings = event;
    sessionStorage.setItem(`${this.componentName}${PageEvent.name}`, JSON.stringify(event));
    this.fetch();
  }

  public ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
