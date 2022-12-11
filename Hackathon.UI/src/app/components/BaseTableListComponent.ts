import {PageEvent} from "@angular/material/paginator";
import {AfterViewInit, Directive, OnDestroy} from "@angular/core";
import {PageSettingsDefaults} from "../models/PageSettings";
import {GetListParameters} from "../models/GetListParameters";
import {Subject} from "rxjs";

@Directive()
export abstract class BaseTableListComponent<T> implements AfterViewInit, OnDestroy {

  public items: T[] = [];
  public pageSettings: PageEvent = new PageEvent();

  private readonly componentName: string | undefined;
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

  public abstract getDisplayColumns():string[];
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
